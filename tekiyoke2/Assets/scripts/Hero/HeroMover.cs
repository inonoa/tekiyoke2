using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

///<summary>最終的には各機能をまとめる役割と渉外担当みたいな役割とだけを持たせたい</summary>
public class HeroMover : MonoBehaviour
{
    #region デバッグ用

    [SerializeField]
    bool isInDebug = false;
    void Log4Debug(){
        string txt = States.Peek().ToString() + "\n"
                   + "Velocity: " + velocity.ToString() + "\n"
                   + "KeyDirection: " + KeyDirection.ToString() + "\n"
                   + "EyeToRight: " + EyeToRight.ToString() + "\n"
                   + "IsOnGround: " + IsOnGround.ToString() + "\n";
        Debug.Log(txt);
    }

    #endregion

    #region 移動関係の(だいたい)定数
    public static float moveSpeed = 15;
    public static readonly float gravity = 1.7f;
    public static readonly float blinkPeriodSec = 0.2f;

    #endregion

    #region 操作・移動関係

    ///<summary> trueだと一切動かない(落ちてる最中でもそこで浮き続ける) </summary>
    public bool IsFrozen { get; set; } = false;
    ///<summary>操作を受け付けるかどうか。空中でfalseになってても落ちはする</summary>
    public bool CanMove { get; set; } = true;
    public bool IsOnGround{ get => groundChecker.IsOnGround; }
    public int FramesSinceTakeOff{ get => groundChecker.FramesSinceTakeOff; }
    public bool IsOnSakamichi{ get => sakamichiChecker.OnSakamichi; }
    public bool IsOnSakamichiR{ get => sakamichiChecker.OnSakamichiR; }
    public bool IsOnSakamichiL{ get => sakamichiChecker.OnSakamichiL; }
    public bool CanKickFromWallL{ get => wallCheckerL.CanKick; }
    public bool CanKickFromWallR{ get => wallCheckerR.CanKick; }

    ///<summary>主人公の目はどちらに向いているか(移動方向とは必ずしも一致しない)
    ///(壁キック中は変則的かも…) (これKeyDirectionといい感じに統合したほうがいいかもな…)
    ///(velocity, KeyDirection参照)</summary>
    public bool EyeToRight{ get; set; } = true;

    ///<summary>実際に移動している方向(ワープした場合は知らん) (EyeToright, KeyDiretion参照)</summary>
    public (float x, float y) velocity = (0,0);

    ///<summary>過去1000フレーム分の位置を記録</summary>
    public readonly RingBuffer<Vector3> pastPoss = new RingBuffer<Vector3>(new Vector3());

    ///<summary>何も障害などが無ければ物理演算後にはこの位置にいるはず</summary>
    public (float x, float y) expectedPosition;

    ///<summary>移動床とかの外部からの移動をつかさどる？</summary>
    public Dictionary<MonoBehaviour, Vector2> additionalVelocities = new Dictionary<MonoBehaviour, Vector2>();

    ///<summary>余韻と言うか一定時間引きずり続けるスピードをアレする</summary>
    public List<ISpeedResidue> speedResidues = new List<ISpeedResidue>();

    ///<summary>このフレームで方向キーの押されている方向 (EyeToRight, velocity参照)</summary>
    public int KeyDirection{ get; private set; } = 0;

    ///<summary>指定した値だけ位置をずらす。timeScaleの影響を受けます</summary>
    public void MovePos(float vx, float vy){
        rigidbody.MovePosition(new Vector2(
            transform.position.x + vx*Time.timeScale,
            transform.position.y + vy*Time.timeScale
        ));
    }

    ///<summary>指定した値に位置が移動。timeScaleの影響を受けません</summary>
    public void WarpPos(float x, float y){
        transform.position = new Vector3(x,y,transform.position.z);
    }
    ///<summary>前フレームでの向きと今フレームの入力から移動方向を決定</summary>
    void UpdateMoveDirection(){
        if(CanMove){
            
            //右ボタンを押したとき右に動く
            if(input.GetButtonDown(ButtonCode.Right) && KeyDirection!=1){
                States.Peek().Try2StartMove(true);
                KeyDirection = 1;
                EyeToRight = true;
    
            //左ボタンを押したときに左に動く
            }else if(input.GetButtonDown(ButtonCode.Left) && KeyDirection!=-1){
                States.Peek().Try2StartMove(false);
                KeyDirection = -1;
                EyeToRight = false;
    
            //右ボタンを離したときはさっきまで動いていた向きによって挙動が変わる
            }else if(input.GetButtonUp(ButtonCode.Right) && KeyDirection==1){
            
                if(input.GetButton(ButtonCode.Left)){
                    States.Peek().Try2StartMove(false);
                    KeyDirection = -1;
                    EyeToRight = false;
    
                }else{
                    States.Peek().Try2EndMove();
                    KeyDirection = 0;
                }
    
            //左ボタンを離したときはさっきまで動いていた向きによって挙動が変わる
            }else if(input.GetButtonUp(ButtonCode.Left) && KeyDirection==-1){
            
                if(input.GetButton(ButtonCode.Right)){
                    States.Peek().Try2StartMove(true);
                    KeyDirection = 1;
                    EyeToRight = true;
    
                }else{
                    States.Peek().Try2EndMove();
                    KeyDirection = 0;
                }
            }

        }
    }
    public event EventHandler jumped;
    public void Jumped(bool isFromGround ,bool isKick)
        => jumped?.Invoke(this, new HeroJumpedEventArgs(isFromGround, isKick));

    #endregion

    #region 別クラスで持っている情報

    ///<summary>一応過去の(特に直前の)状態を見るためにStackに積んでるけど必要か…？</summary>
    public Stack<IHeroState> States { get; set; } = new Stack<IHeroState>();
    ///<summary>直前フレームの状態が入っているはず(大半の場合現在の状態と同じ)</summary>
    IHeroState lastState;
    [HideInInspector] public CameraController cmrCntr;
    [HideInInspector] public HpCntr hpcntr;
    [HideInInspector] public HeroObjsHolder4States objsHolderForStates;
    [SerializeField] GroundChecker groundChecker;
    SakamichiChecker sakamichiChecker;
    [SerializeField] WallCheckerL wallCheckerL;
    [SerializeField] WallCheckerR wallCheckerR;
    SavePositionManager savePositionManager;
    IAskedInput input;

    [SerializeField] GetDPinEnemy getDPinEnemy;
    public GetDPinEnemy GetDPinEnemy => getDPinEnemy;


    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator anim;
    [HideInInspector] public new Rigidbody2D rigidbody;
    Chishibuki chishibuki;
    
    #endregion

    #region ダメージとか

    ///<summary>HPの増減はすべてここから。(全部HPCntrに通します) (これ何のためにプロパティやめたのかわかんねえな)</summary>
    private int HP{
        get => hpcntr.HP;
        set => hpcntr.ChangeHP(value);
    }

    ///<summary>falseだと無敵になる</summary>
    public bool CanBeDamaged{ get => hpcntr.CanBeDamaged; set => hpcntr.CanBeDamaged = value; }
    
    ///<summary>敵からのダメージ等。ノックバックなどが入る予定(あれ？)</summary>
    ///<param name="damage">与えるダメージを書く。1を指定すると100->99,1->0になったりします</param>
    public void Damage(int damage){
        if(CanBeDamaged){
            Tokitome.SetTime(1);
            HP = HP - damage;
            cmrCntr.Reset();
        }
    }
    public void BendBack(object sender, EventArgs e){
        States.Push(new StateBend(this));
        ParticleSystem ps = transform.Find("Particle System").GetComponent<ParticleSystem>();
        ps.Play();
        chishibuki.StartCoroutine("StartChishibuki");
        StartCoroutine(Blink());
    }

    IEnumerator Blink(){
        yield return new WaitForSeconds(0.3f);

        while(true){

            if(hpcntr.CanBeDamaged) yield break;
            spriteRenderer.material.SetFloat("_Alpha", 0);

            yield return new WaitForSeconds(blinkPeriodSec/2);

            spriteRenderer.material.SetFloat("_Alpha", 1);
            if(hpcntr.CanBeDamaged) yield break;

            yield return new WaitForSeconds(blinkPeriodSec/2);
        }
    }

    ///<summary>リスポーン</summary>
    void Die(){
        MemoryOverDeath.Instance.SaveOnDeath();
        GameTimeCounter.CurrentInstance.DoesTick = false;
        SceneTransition.Start2ChangeState(SceneManager.GetActiveScene().name, SceneTransition.TransitionType.HeroDied);
    }
    ///<summary>落下死、実装が強引でうーん</summary>
    public void Drop(){
        Damage(3);
        if(CanBeDamaged){
            CanMove = false;
            velocity = (0, -15);
        }
    }

    ///<summary>HPCntrからの死亡イベントをこう良い感じに…</summary>
    void ReceiveDeath(object sender, EventArgs e) => Die();

    #endregion

    #region 勝手に呼ばれる関数群

    ///<summary>他のオブジェクトのStart()内でCurrentHeroを参照したい時があり、
    ///Start()内でcurrentHeroを設定すると実行順によっては前シーンのHeroを参照してしまうため</summary>
    void Awake(){
        HeroDefiner.currentHero = this;
    }

    void Start()
    {
        States.Push(new StateWait(this));
        lastState = States.Peek();

        cmrCntr = CameraController.CurrentCamera;
        input   = InputManager.Instance;
        chishibuki = GameUIManager.CurrentInstance.Chishibuki;
        spriteRenderer      = GetComponent<SpriteRenderer>();
        anim                = GetComponent<Animator>();
        rigidbody           = GetComponent<Rigidbody2D>();
        hpcntr              = GetComponent<HpCntr>();
        sakamichiChecker    = GetComponent<SakamichiChecker>();
        savePositionManager = GetComponent<SavePositionManager>();
        objsHolderForStates = GetComponent<HeroObjsHolder4States>();

        hpcntr.die     += ReceiveDeath;
        hpcntr.damaged += BendBack;
        getDPinEnemy.gotDP += (dp, e) => {
            DPManager.Instance.AddDP((float)dp);
            DPManager.Instance.LightGaugePulse();
        };
    }

    ///<summary>SetActive(false)するとアニメーションの状態がリセットされるようなのでとりあえず主人公はステートだけ反映しなおす</summary>
    void OnEnable(){
        if(States.Count>0) States.Peek().Resume();
    }

    void Update()
    {

        if(!IsFrozen){

            if(CanMove){

                //なんとなく入力をまとめて置きたくてここにしているがあまり意味がないような…
                if(input.GetNagaoshiFrames(ButtonCode.Save) == 70) savePositionManager.Try2Save();


                UpdateMoveDirection();

                if(input.GetButtonDown(ButtonCode.Jump)){
                    States.Peek().Try2Jump();
                }

                //面倒だし向きは移動方向と同じでいいからキーは1つでいい気がするが…
                if(input.GetButtonDown(ButtonCode.JetLR)){
                    States.Peek().Try2StartJet();
                }
                if(input.GetButtonUp(ButtonCode.JetLR)){
                    States.Peek().Try2EndJet();
                }
            }
        }

        if(isInDebug) Log4Debug();
    }


    void FixedUpdate(){

        pastPoss.PushFirst(transform.position);
        if(pastPoss.Count > 1000) pastPoss.PopLast();

        if(!IsFrozen){

            if(CanMove){

                if(States.Peek() != lastState){
                    lastState.Exit();
                    States.Peek().Start();
                    lastState = States.Peek();
                }

                States.Peek().Update(); //ここかこれ？
            }

            float vx = velocity.x;
            float vy = velocity.y;
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

    ///<summary>天井に衝突したときに天井に張り付かないようにする</summary>
    void OnCollisionStay2D(Collision2D col){

        if(col.gameObject.tag=="Terrain" && !IsFrozen){
            foreach(ContactPoint2D contact in col.contacts){
                if(contact.normal.y<0){
                    velocity.y = 0;
                    return;
                }
            }
        }
    }

    #endregion
}
