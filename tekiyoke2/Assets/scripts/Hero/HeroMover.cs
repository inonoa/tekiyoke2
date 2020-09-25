using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UniRx;
using DG.Tweening;

///<summary>最終的には各機能をまとめる役割と渉外担当みたいな役割とだけを持たせたい</summary>
public class HeroMover : MonoBehaviour
{
    public Stack<HeroState> States;


    #region 移動関係の(だいたい)定数
    public static float moveSpeed = 15;
    public static readonly float gravity = 1.7f;
    public static readonly float blinkPeriodSec = 0.2f;

    #endregion

    #region 操作・移動関係

    public bool IsFrozen { get; set; } = false;
    public bool CanMove { get; set; } = true;

    public bool IsOnGround         => groundChecker.IsOnGround;
    public int  FramesSinceTakeOff => groundChecker.FramesSinceTakeOff;
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

    public int KeyDirection{ get; private set; } = 0;
    public bool WantsToGoRight{ get; private set; } = true;



    ///<summary>指定した値だけ位置をずらす。timeScaleの影響を受けます</summary>
    void MovePos(float vx, float vy)
    {
        Rigidbody.MovePosition(new Vector2(
            transform.position.x + vx*Time.timeScale,
            transform.position.y + vy*Time.timeScale
        ));
    }

    ///<summary>指定した値に位置が移動。timeScaleの影響を受けません</summary>
    public void WarpPos(float x, float y){
        transform.position = new Vector3(x,y,transform.position.z);
    }
    

    void UpdateMoveDirection()
    {
        if(!CanMove) return;
        
        if(Input.GetButtonDown(ButtonCode.Right)      && KeyDirection != 1)
        {
            KeyDirection = 1;
            WantsToGoRight = true;
        }
        else if(Input.GetButtonDown(ButtonCode.Left)  && KeyDirection != -1)
        {
            KeyDirection = -1;
            WantsToGoRight = false;
        }

        //ボタンを離したときはさっきまで動いていた向きによって挙動が変わる
        else if(Input.GetButtonUp(ButtonCode.Right) && KeyDirection == 1)
        {
            if(Input.GetButton(ButtonCode.Left))
            {
                KeyDirection = -1;
                WantsToGoRight = false;
            }
            else
            {
                KeyDirection = 0;
            }
        }
        else if(Input.GetButtonUp(ButtonCode.Left)  && KeyDirection == -1)
        {
            if(Input.GetButton(ButtonCode.Right))
            {
                KeyDirection = 1;
                WantsToGoRight = true;
            }
            else
            {
                KeyDirection = 0;
            }
        }
    }

    public event EventHandler jumped;
    public void Jumped(bool isFromGround ,bool isKick)
        => jumped?.Invoke(this, new HeroJumpedEventArgs(isFromGround, isKick));

    #endregion

    #region 別クラスで持っている情報
    public CameraController CmrCntr{ get; private set; }
    public HpCntr HpCntr{ get; private set; }
    public HeroObjsHolder4States ObjsHolderForStates{ get; private set; }
    [SerializeField] SoundGroup _SoundGroup;
    public SoundGroup SoundGroup => _SoundGroup;

    [field: SerializeField] [field: RenameField("WindSounds")]
    public WindSoundController WindSounds{ get; private set; }
    
    [SerializeField] GroundChecker groundChecker;
    SakamichiChecker sakamichiChecker;
    [SerializeField] WallCheckerL wallCheckerL;
    [SerializeField] WallCheckerR wallCheckerR;
    SavePositionManager savePositionManager;
    public IAskedInput Input{ get; private set; }

    [SerializeField] GetDPinEnemy getDPinEnemy;
    public GetDPinEnemy GetDPinEnemy => getDPinEnemy;

    [SerializeField] HeroParameters _Parameters;
    public HeroParameters Parameters => _Parameters;

    HeroStateBase currrentState;
    public string CurrentStateStr() => currrentState.ToString();


    public SpriteRenderer SpriteRenderer{ get; private set; }
    public Animator Anim{ get; private set; } //いずれprivateにする
    public Rigidbody2D Rigidbody{ get; private set; }

    Chishibuki chishibuki;
    
    #endregion

    #region ダメージとか
    void ChangeHP(int value) => HpCntr.ChangeHP(value);
    public int HP => HpCntr.HP;

    ///<summary>falseだと無敵になる</summary>
    public bool CanBeDamaged{ get => HpCntr.CanBeDamaged; set => HpCntr.CanBeDamaged = value; }
    
    ///<summary>敵からのダメージ等。ノックバックなどが入る予定(あれ？)</summary>
    ///<param name="damage">与えるダメージを書く。1を指定すると100->99,1->0になったりします</param>
    public void Damage(int damage, DamageType type)
    {
        if(! CanBeDamaged) return;

        Tokitome.SetTime(1);
        ChangeHP(HP - damage);
        CmrCntr.Reset();
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
    }

    void BendBack(){
        ChangeState(new StateBend_());
        ParticleSystem ps = transform.Find("Particle System").GetComponent<ParticleSystem>();
        ps.Play();
        chishibuki.StartCoroutine("StartChishibuki");
        StartCoroutine(Blink());
    }

    IEnumerator Blink(){
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
    void Die(){
        MemoryOverDeath.Instance.SaveOnDeath();
        GameTimeCounter.CurrentInstance.DoesTick = false;
        Tokitome.SetTime(0.2f);
        SceneTransition.Start2ChangeScene(SceneManager.GetActiveScene().name, SceneTransition.TransitionType.HeroDied);
    }

    public void RecoverHP(int amount) => ChangeHP(HP + amount);

    #endregion

    #region 勝手に呼ばれる関数群

    ///<summary>他のオブジェクトのStart()内でCurrentHeroを参照したい時があり、
    ///Start()内でcurrentHeroを設定すると実行順によっては前シーンのHeroを参照してしまうため</summary>
    void Awake(){
        HeroDefiner.currentHero = this;
    }

    void Start()
    {

        CmrCntr = CameraController.CurrentCamera;
        Input   = ServicesLocator.Instance.GetInput();
        chishibuki = GameUIManager.CurrentInstance.Chishibuki;
        SpriteRenderer      = GetComponent<SpriteRenderer>();
        Anim                = GetComponent<Animator>();
        Rigidbody           = GetComponent<Rigidbody2D>();
        HpCntr              = GetComponent<HpCntr>();
        sakamichiChecker    = GetComponent<SakamichiChecker>();
        savePositionManager = GetComponent<SavePositionManager>();
        ObjsHolderForStates = GetComponent<HeroObjsHolder4States>();
        GetComponent<JetManager>().Init(Input, this);

        currrentState = new StateWait_();
        currrentState.Enter(this);

        getDPinEnemy.gotDP += (dp, e) => {
            DPManager.Instance.AddDP((float)dp);
            DPManager.Instance.LightGaugePulse();
        };

        //ポーズ対応してない
        Observable.Interval(TimeSpan.FromSeconds(0.075f)) //即値
            .Subscribe(_ =>
            {
                ObjsHolderForStates.AfterimagePool.ActivateOne("")
                    .GetComponent<SpriteRenderer>()
                    .sprite = SpriteRenderer.sprite;
            })
            .AddTo(this);
    }

    ///<summary>SetActive(false)するとアニメーションの状態がリセットされるようなのでとりあえず主人公はステートだけ反映しなおす</summary>
    void OnEnable()
    {
        currrentState?.Resume(this);
    }

    void OnDisable(){
        SoundGroup.StopAll();
    }

    void Update()
    {

        if(!IsFrozen)
        {

            if(CanMove)
            {

                //なんとなく入力をまとめて置きたくてここにしているがあまり意味がないような…
                if(Input.GetNagaoshiFrames(ButtonCode.Save) == 70) savePositionManager.Try2Save();


                UpdateMoveDirection();

                HeroStateBase next = currrentState.HandleInput(this, Input);
                if(next != currrentState)
                {
                    ChangeState(next);
                }
            }
        }
    }


    void FixedUpdate(){

        pastPoss.PushFirst(transform.position);
        if(pastPoss.Count > 1000) pastPoss.PopLast();

        if(!IsFrozen){

            HeroStateBase next = currrentState.Update_(this, Time.fixedDeltaTime);
            if(next != currrentState)
            {
                ChangeState(next);
            }

            float vx = velocity.X;
            float vy = velocity.Y;
            foreach(Vector2 vi in additionalVelocities.Values){
                vx += vi.x;
                vy += vi.y;
            }

            speedResidues.RemoveAll(residue => residue.UpdateSpeed(this));
            foreach(ISpeedResidue residue in speedResidues){
                vx += residue.SpeedX;
                vy += residue.SpeedY;
            }

            MovePos(vx, vy);
            expectedPosition.x = transform.position.x + vx*Time.timeScale;
            expectedPosition.y = transform.position.y + vy*Time.timeScale;

        }
    }

    void ChangeState(HeroStateBase next)
    {
        currrentState.Exit(this);
        currrentState = next;
        currrentState.Enter(this);
    }

    public IObservable<Unit> Jet(float charge_0_1)
    {
        var state = new StateJet_(charge_0_1);
        //Changeこれでええんかな
        ChangeState(state);
        return state.OnJetCompleted;
    }

    ///<summary>天井に衝突したときに天井に張り付かないようにする</summary>
    void OnCollisionStay2D(Collision2D col){

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
