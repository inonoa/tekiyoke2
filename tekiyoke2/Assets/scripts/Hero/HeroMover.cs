using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UniRx;
using DG.Tweening;
using System.Linq;
using Sirenix.OdinInspector;


///<summary>最終的には各機能をまとめる役割と渉外担当みたいな役割とだけを持たせたい</summary>
public class HeroMover : MonoBehaviour
{

    #region 操作・移動関係

    public bool IsFrozen { get; set; } = false;
    public bool CanMove { get; set; } = true;

    public bool IsOnGround         => groundChecker.IsOnGround;
    public bool IsOnSakamichi      => sakamichiChecker.OnSakamichi;
    public bool IsOnSakamichiR     => sakamichiChecker.OnSakamichiR;
    public bool IsOnSakamichiL     => sakamichiChecker.OnSakamichiL;
    public bool CanKickFromWallL   => wallCheckerL.CanKick;
    public bool CanKickFromWallR   => wallCheckerR.CanKick;


    ///<summary>実際に移動している方向(ワープした場合は知らん) (EyeToright, KeyDiretion参照)</summary>
    public HeroVelocity velocity = new HeroVelocity(0,0);

    ///<summary>過去1000フレーム分の位置を記録</summary>
    public readonly RingBuffer<Vector3> pastPoss = new RingBuffer<Vector3>(new Vector3());

    ///<summary>何も障害などが無ければ物理演算後にはこの位置にいるはず</summary>
    public (float x, float y) expectedPosition;

    ///<summary>移動床とかの外部からの移動をつかさどる？</summary>
    public Dictionary<MonoBehaviour, Vector2> additionalVelocities = new Dictionary<MonoBehaviour, Vector2>();

    ///<summary>余韻と言うか一定時間引きずり続けるスピードをアレする</summary>
    public List<ISpeedResidue> speedResidues = new List<ISpeedResidue>();
    
    [field: RenameField("Key Direction")]     [field: ReadOnly] [field: SerializeField]
    public int KeyDirection{ get; private set; } = 0;

    [field: RenameField("Wants To Go Right")] [field: ReadOnly] [field: SerializeField]
    public bool WantsToGoRight{ get; private set; } = true;



    ///<summary>指定した値だけ位置をずらす。timeScaleの影響を受けます</summary>
    void MovePos(float vx, float vy)
    {
        Rigidbody.MovePosition(new Vector2(
            transform.position.x + vx * TimeManager.TimeScaleAroundHero,
            transform.position.y + vy * TimeManager.TimeScaleAroundHero
        ));
    }

    ///<summary>指定した値に位置が移動。timeScaleの影響を受けません</summary>
    public void WarpPos(float x, float y)
    {
        transform.position = new Vector3(x,y,transform.position.z);
    }
    
    bool leftLast  = false;
    bool rightLast = false;
    void UpdateMoveDirection()
    {
        if(!CanMove) return;

        bool left  = Input.GetButton(ButtonCode.Left);
        bool right = Input.GetButton(ButtonCode.Right);
        
        if(     !rightLast && right && KeyDirection != 1)
        {
            KeyDirection = 1;
            WantsToGoRight = true;
        }
        else if(!leftLast  && left  && KeyDirection != -1)
        {
            KeyDirection = -1;
            WantsToGoRight = false;
        }

        //ボタンを離したときはさっきまで動いていた向きによって挙動が変わる
        else if(rightLast && !right && KeyDirection == 1)
        {
            if(left)
            {
                KeyDirection = -1;
                WantsToGoRight = false;
            }
            else
            {
                KeyDirection = 0;
            }
        }
        else if(leftLast  && !left  && KeyDirection == -1)
        {
            if(right)
            {
                KeyDirection = 1;
                WantsToGoRight = true;
            }
            else
            {
                KeyDirection = 0;
            }
        }

        leftLast  = left;
        rightLast = right;
    }

    public event EventHandler jumped;
    public void Jumped(bool isFromGround ,bool isKick)
        => jumped?.Invoke(this, new HeroJumpedEventArgs(isFromGround, isKick));

    #endregion

    #region 別クラスで持っている情報
    public CameraController CmrCntr{ get; private set; }
    public TimeManager TimeManager{ get; private set; }
    public HpCntr HpCntr{ get; private set; }
    public HeroObjsHolder4States ObjsHolderForStates{ get; private set; }
    [SerializeField] SoundGroup _SoundGroup;
    public SoundGroup SoundGroup => _SoundGroup;

    [field: SerializeField] [field: RenameField("WindSounds")]
    public WindSoundController WindSounds{ get; private set; }
    
    [SerializeField] GroundChecker groundChecker;
    SakamichiChecker sakamichiChecker;
    [SerializeField] WallChecker wallCheckerL;
    [SerializeField] WallChecker wallCheckerR;
    SavePositionManager savePositionManager;
    public IAskedInput Input{ get; private set; }

    [SerializeField] GetDPinEnemy getDPinEnemy;
    public GetDPinEnemy GetDPinEnemy => getDPinEnemy;

    [SerializeField] HeroParameters _Parameters;
    [SerializeField] HeroParameters _ParametersInDraftMode;
    public HeroParameters Parameters
    {
        get
        {
            return draftModeManager.InDraftMode ? _ParametersInDraftMode : _Parameters;
        }
    }

    public JetManager JetManager{ get; private set; }

    DraftModeManager draftModeManager;

    HeroState currrentState;
    public string CurrentStateStr() => currrentState.ToString();


    public SpriteRenderer SpriteRenderer{ get; private set; }
    public Rigidbody2D Rigidbody{ get; private set; }
    public Transform Transform{ get; private set; }

    [SerializeField] HeroAnim anim;
    public void SetAnim(string id)         => anim.SetTrigger(id);
    public void SetAnimManually(string id) => anim.SetTriggerManually(id);

    Chishibuki chishibuki;
    
    #endregion

    #region ダメージとか
    void ChangeHP(int value) => HpCntr.ChangeHP(value);
    public int HP => HpCntr.HP;
    public bool IsLiving => HpCntr.HP > 0;

    
    public bool CanBeDamaged => HpCntr.CanBeDamaged;

    Subject<int> _OnDamaged = new Subject<int>();
    public IObservable<int> OnDamaged => _OnDamaged;
    
    ///<summary>敵からのダメージ等。ノックバックなどが入る予定(あれ？)</summary>
    ///<param name="damage">与えるダメージを書く。1を指定すると100->99,1->0になったりします</param>
    public void Damage(int damage, DamageType type)
    {
        if(! IsLiving) return;
        if(! CanBeDamaged && type != DamageType.Drop) return;

        TimeManager.Reset();
        ChangeHP(HP - damage);
        CmrCntr.Reset();
        draftModeManager.TryExit();
        SoundGroup.Play(HP==0 ? "Die" : "Damage");

        if(HP <= 0) Die();

        switch(type)
        {
        case DamageType.Normal:
        {
            BendBack();
        }
        break;
        case DamageType.Drop:
        {
            CanMove = false;
            velocity = new HeroVelocity(0, -15);
        }
        break;
        }

        _OnDamaged.OnNext(HP);
    }

    void BendBack()
    {
        ChangeState(new StateBend());
        ParticleSystem ps = transform.Find("Particle System").GetComponent<ParticleSystem>();
        ps.Play();
        chishibuki.StartChishibuki();
        this.StartPausableCoroutine(Blink()).AddTo(this);
    }

    static readonly float blinkPeriodSec = 0.2f;
    IEnumerator Blink()
    {
        yield return new WaitForSeconds(0.3f);

        while(true){

            if(HpCntr.CanBeDamaged) yield break;
            SpriteRenderer.material.SetFloat("_Alpha", 0);

            yield return new WaitForSeconds(blinkPeriodSec/2);

            SpriteRenderer.material.SetFloat("_Alpha", 1);
            if(HpCntr.CanBeDamaged) yield break;

            yield return new WaitForSeconds(blinkPeriodSec/2);
        }
    }


    ///<summary>リスポーン</summary>
    void Die()
    {
        MemoryOverDeath.Instance.SaveOnDeath();
        GameTimeCounter.CurrentInstance.DoesTick = false;
        TimeManager.SetTimeScale(TimeEffectType.Die, 0.2f);
        SceneTransition.Start2ChangeScene(SceneManager.GetActiveScene().name, SceneTransition.TransitionType.HeroDied);
        draftModeManager.TryExit();
    }

    public void RecoverHP(int amount) => ChangeHP(HP + amount);

    #endregion

    List<Tween> tweens = new List<Tween>();

    #region 勝手に呼ばれる関数群

    ///<summary>他のオブジェクトのStart()内でCurrentHeroを参照したい時があり、
    ///Start()内でcurrentHeroを設定すると実行順によっては前シーンのHeroを参照してしまうため</summary>
    void Awake()
    {
        HeroDefiner.currentHero = this;
    }

    void Start()
    {

        CmrCntr = CameraController.CurrentCamera;
        TimeManager = TimeManager.CurrentInstance;
        Input   = ServicesLocator.Instance.GetInput();
        chishibuki = GameUIManager.CurrentInstance.Chishibuki;
        SpriteRenderer      = GetComponent<SpriteRenderer>();
        Rigidbody           = GetComponent<Rigidbody2D>();
        Transform           = GetComponent<Transform>();
        HpCntr              = GetComponent<HpCntr>();
        sakamichiChecker    = GetComponent<SakamichiChecker>();
        savePositionManager = GetComponent<SavePositionManager>();
        ObjsHolderForStates = GetComponent<HeroObjsHolder4States>();
        JetManager          = GetComponent<JetManager>();
        draftModeManager    = GetComponent<DraftModeManager>();

        JetManager.Init(Input, this);

        currrentState = new StateWait();
        currrentState.Enter(this);

        getDPinEnemy.gotDP += (dp, e) => {
            DPManager.Instance.AddDP((float)dp);
            DPManager.Instance.LightGaugePulse();
        };

        InitEffect();
    }

    void InitEffect()
    {
        Tween afterImages = DOVirtual.DelayedCall(0.075f, () =>
        {
            ObjsHolderForStates.AfterimagePool.ActivateOne("")
                .GetComponent<SpriteRenderer>()
                .sprite = SpriteRenderer.sprite;
        }, ignoreTimeScale: false)
        .SetLoops(-1)
        .AsHeros();
        afterImages.GetPausable().AddTo(this);
        tweens.Add(afterImages);
    }

    ///<summary>SetActive(false)するとアニメーションの状態がリセットされるようなのでとりあえず主人公はステートだけ反映しなおす</summary>
    void OnEnable()
    {
        currrentState?.Resume(this);
    }

    void OnDisable()
    {
        SoundGroup.StopAll();
    }

    void OnDestroy()
    {
        tweens.ForEach(tw => tw.Kill());
    }

    void Update()
    {

        if(!IsFrozen)
        {

            if(CanMove)
            {

                // なんとなく入力をまとめて置きたくてここにしているがあまり意味がないような…
                // if(Input.GetNagaoshiFrames(ButtonCode.Save) == 70) savePositionManager.Try2Save();

                if(Input.GetButtonDown(ButtonCode.Zone))
                {
                    if(draftModeManager.InDraftMode) draftModeManager.TryExit();
                    else                             draftModeManager.TryEnter();
                }

                UpdateMoveDirection();

                HeroState next = currrentState.HandleInput(this, Input);
                if(next != currrentState)
                {
                    ChangeState(next);
                }
            }
        }
    }


    void FixedUpdate()
    {

        pastPoss.PushFirst(transform.position);
        if(pastPoss.Count > 1000) pastPoss.PopLast();

        if(IsFrozen) return;

        HeroState next = currrentState.Update_(this, TimeManager.FixedDeltaTimeAroundHero);
        if(next != currrentState)
        {
            ChangeState(next);
        }

        Vector2 baseVel = velocity.ToVector2();

        Vector2 added = additionalVelocities.Aggregate(baseVel, (cur, kvp) => cur + kvp.Value);

        Vector2 residueApplied = speedResidues
            .Aggregate(
                added,
                (cur, residue) => residue.UpdateVel(cur, TimeManager.FixedDeltaTimeAroundHero, this)
            );
        speedResidues.RemoveAll(residue => !residue.IsActive);

        MovePos(residueApplied.x, residueApplied.y);
        expectedPosition.x = transform.position.x + residueApplied.x * TimeManager.TimeScaleAroundHero;
        expectedPosition.y = transform.position.y + residueApplied.y * TimeManager.TimeScaleAroundHero;
    }

    void ChangeState(HeroState next)
    {
        currrentState.Exit(this);
        currrentState = next;
        currrentState.Enter(this);
    }

    public IObservable<Unit> Jet(float charge_0_1)
    {
        var state = new StateJet(charge_0_1);
        //Changeこれでええんかな
        ChangeState(state);
        return state.OnJetCompleted;
    }

    public void PushedByBaneYoko(bool toRight, float force)
    {
        if(KeyDirection == 0)
        {
            WantsToGoRight = toRight;
            anim.SetTriggerManually(toRight ? "runr" : "runl");
        }
        speedResidues.Add(new BaneResidue(toRight, force, 0.8f));
    }

    public void OnGoal()
    {
        ChangeState(new StateRun());
        draftModeManager.TryExit();
        JetManager.Cancel();
    }

    public void ForceJump(float force = -1)
    {
        ChangeState(new StateJump(force: force));
    }

    ///<summary>天井に衝突したときに天井に張り付かないようにする</summary>
    void OnCollisionStay2D(Collision2D col)
    {

        if(col.gameObject.tag=="Terrain" && !IsFrozen){
            foreach(ContactPoint2D contact in col.contacts){
                if(contact.normal.y<0){
                    velocity.Y = 0;
                    return;
                }
            }
        }
    }

    #endregion
}
