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
    public bool CanKickFromWallL   => wallCheckerL.CanKick;
    public bool CanKickFromWallR   => wallCheckerR.CanKick;

    public IObservable<Unit> OnLand => groundChecker.OnLand;

    [field: FoldoutGroup(COMP), SerializeField, LabelText("Sakamichi Sensors")]
    public SakamichiSensors SakamichiSensors{ get; private set; }


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

    public void ForceChangeWantsToGoRight(bool right) => WantsToGoRight = right;



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

    Subject<(bool isFromGround, bool isKick)> _OnJumped = new Subject<(bool isFromGround, bool isKick)>();
    public IObservable<(bool isFromGround, bool isKick)> OnJumped => _OnJumped;
    public void Jumped(bool isFromGround, bool isKick) => _OnJumped.OnNext((isFromGround, isKick));

    #endregion

    #region 別クラスで持っている情報
    public CameraController CmrCntr{ get; private set; }
    public TimeManager TimeManager{ get; private set; }

    [field: FoldoutGroup(COMP), SerializeField, RenameField("HP Controller")]
    public HPController HPController{ get; private set; }

    [field: FoldoutGroup(COMP), SerializeField, RenameField("Muteki Manager")]
    public HeroMutekiManager MutekiManager{ get; private set; }
    public HeroObjsHolder4States ObjsHolderForStates{ get; private set; }
    [FoldoutGroup(COMP), SerializeField] SoundGroup _SoundGroup;
    public SoundGroup SoundGroup => _SoundGroup;

    [field: FoldoutGroup(COMP), SerializeField, RenameField("WindSounds")]
    public WindSoundController WindSounds{ get; private set; }
    
    const string COMP = "包含";
    [FoldoutGroup(COMP), SerializeField] GroundChecker groundChecker;
    [FoldoutGroup(COMP), SerializeField] WallChecker wallCheckerL;
    [FoldoutGroup(COMP), SerializeField] WallChecker wallCheckerR;
    [FoldoutGroup(COMP), SerializeField] JumpCounter _JumpCounter;
    public bool CanJumpInAir => _JumpCounter.CanJumpInAir;
    SavePositionManager savePositionManager;
    public IAskedInput Input{ get; private set; }

    [FoldoutGroup(COMP), SerializeField] GetDPinEnemy getDPinEnemy;
    public GetDPinEnemy GetDPinEnemy => getDPinEnemy;

    const string PARAM = "パラメータ類";
    [BoxGroup(PARAM), SerializeField] HeroParameters _Parameters;
    [BoxGroup(PARAM), SerializeField] HeroParameters _ParametersInDraftMode;
    public HeroParameters Parameters
    {
        get
        {
            return draftModeManager.InDraftMode ? _ParametersInDraftMode : _Parameters;
        }
    }

    [field: BoxGroup(PARAM), SerializeField, LabelText("Draft Mode Params")]
    public DraftModeParams DraftModeParams { get; private set; }

    public JetManager JetManager{ get; private set; }

    DraftModeManager draftModeManager;

    HeroState currentState;
    public string CurrentStateStr() => currentState.ToString();


    public SpriteRenderer SpriteRenderer{ get; private set; }
    public Rigidbody2D Rigidbody{ get; private set; }
    public Transform Transform{ get; private set; }

    [FoldoutGroup(COMP), SerializeField] HeroAnim anim;
    public void SetAnim(string id)         => anim.SetTrigger(id);
    public void SetAnimManually(string id) => anim.SetTriggerManually(id);

    Chishibuki chishibuki;
    
    #endregion

    #region ダメージとか
    void ChangeHP(int value) => HPController.ChangeHP(value);
    public int HP => HPController.HP;
    public bool IsLiving => HPController.HP > 0;

    
    public bool CanBeDamaged => HPController.CanBeDamaged;

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
        case DamageType.Debug:
        {
            //
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

            if(HPController.CanBeDamaged) yield break;
            SpriteRenderer.material.SetFloat("_Alpha", 0);

            yield return new WaitForSeconds(blinkPeriodSec/2);

            SpriteRenderer.material.SetFloat("_Alpha", 1);
            if(HPController.CanBeDamaged) yield break;

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
        TimeManager = TimeManager.Current;
        Input   = ServicesLocator.Instance.GetInput();
        chishibuki = GameUIManager.CurrentInstance.Chishibuki;
        SpriteRenderer      = GetComponent<SpriteRenderer>();
        Rigidbody           = GetComponent<Rigidbody2D>();
        Transform           = GetComponent<Transform>();
        savePositionManager = GetComponent<SavePositionManager>();
        ObjsHolderForStates = GetComponent<HeroObjsHolder4States>();
        JetManager          = GetComponent<JetManager>();
        draftModeManager    = GetComponent<DraftModeManager>();

        JetManager.Init(Input, this);

        currentState = new StateWait();
        currentState.Enter(this);

        getDPinEnemy.gotDP += (dp, e) => {
            GetDP((float)dp);
            DPManager.Instance.LightGaugePulse();
        };

        InitEffect();
    }

    public void GetDP(float dp)
    {
        float actualDP = draftModeManager.InDraftMode ? dp * DraftModeParams.GotDpRate : dp;
        DPManager.Instance.AddDP(actualDP);
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
        currentState?.Resume(this);
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

                if(Input.GetButtonDown(ButtonCode.Save)) Damage(3, DamageType.Debug);

                UpdateMoveDirection();

                HeroState next = currentState.HandleInput(this, Input);
                if(next != currentState)
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

        HeroState next = currentState.Update_(this, TimeManager.FixedDeltaTimeAroundHero);
        if(next != currentState)
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
        currentState.Exit(this);
        currentState = next;
        currentState.Enter(this);
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
        CanMove = false;
        ChangeState(new StateRun());
        draftModeManager.TryExit();
        JetManager.Cancel();
        SoundGroup.FadeOut("Run", 2f);
        WindSounds.FadeOut(2f);
    }

    public void ForceJump(float force = -1)
    {
        ChangeState(new StateJump(force: force));
    }

    ///<summary>天井に衝突したときに天井に張り付かないようにする</summary>
    void OnCollisionStay2D(Collision2D col)
    {
        if(IsFrozen) return;
        
        if(col.gameObject.CompareTag("Terrain"))
        {
            if (col.contacts.Any(contact => contact.normal.y < -0.5f))
            {
                velocity.Y = 0;
            }
        }
    }

    #endregion
}
