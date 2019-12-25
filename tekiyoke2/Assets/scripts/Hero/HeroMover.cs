using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

///<summary>最終的には各機能をまとめる役割と渉外担当みたいな役割とだけを持たせたい</summary>
public class HeroMover : MonoBehaviour
{

    #region 変数と関数

    ///<summary> falseだと一切動かない(落ちてる最中でもそこで浮き続ける) </summary>
    public bool CanMove { get; set; } = true;

    public Stack<IHeroState> States { get; set; } = new Stack<IHeroState>();

    public static float moveSpeed = 20;

    ///<summary>入力に応じて-1,0,1のどれかを返す</summary>
    private int MoveDirection2Sign{
        //これGetAxisでよくね？？
        get{
            if(Input.GetKey(KeyCode.RightArrow)) return 1;
            if(Input.GetKey(KeyCode.LeftArrow)) return -1;
            return 0;
        }
    }

    int lastDirection = 0;
    public static float jumpSpeed = 40;
    public static float bendBackSpeedX = 15;
    public static float bendBackSpeedY = 20;

    ///<summary>壁キック後の横方向の移動速度</summary>
    private float _SpeedXAfterKick = 0;
    ///<summary>壁キック後の横方向の移動速度</summary>
    public float SpeedXAfterKick{
        get{ return _SpeedXAfterKick; }
        set { _SpeedXAfterKick = value; }
    }
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public new Rigidbody2D rigidbody;
    public CameraController cmrCntr;
    public bool isBendingBack = false;

    public GameObject curtain;

    ///<summary>HPの増減はすべてここから。(全部HPCntrに通します)</summary>
    private int HP{
        get{return hpcntr.HP;}
        set{hpcntr.HP = value;}
    }
    
    ///<summary>敵からのダメージ等。ノックバックなどが入る予定</summary>
    ///<param name="damage">与えるダメージを書く。1を指定すると100->99,1->0になったりします</param>
    public void Damage(int damage){
        HP = HP - damage;
        cmrCntr.Reset();
        dashcntr.Reset();
    }

    ///<summary>リスポーン</summary>
    public void Die(){
        if(curtain.GetComponent<Curtain4DeathMover>().state!=Curtain4DeathMover.CState.Dying){
            curtain.SetActive(true);
            curtain.GetComponent<Curtain4DeathMover>().state = Curtain4DeathMover.CState.Dying;
            curtain.GetComponent<Curtain4DeathMover>().ResetPosition();
        }
    }

    ///<summary>HPCntrからの死亡イベントをこう良い感じに…</summary>
    public void ReceiveDeath(object sender, EventArgs e) => Die();

    public void Respawn(object sender, EventArgs e){
        transform.position = new Vector3(0,-200);
        hpcntr.FullRecover();
    }

    public HpCntr hpcntr;

    public DashController dashcntr;


    ///<summary>現状ジャンプにしてあるがそのままにしてはおけない</summary>
    public void BendBack(object sender, EventArgs e){
        
    }

    public event EventHandler jumped;

    public (float x, float y) velocity = (0,0);
    IHeroState lastState;

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

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        States.Push(new StateWait(this));
        lastState = States.Peek();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        hpcntr = GetComponent<HpCntr>();
        dashcntr = GetComponent<DashController>();
        hpcntr.die += ReceiveDeath;
        hpcntr.damaged += BendBack;
        curtain.GetComponent<Curtain4DeathMover>().heroRespawn += Respawn;

        HeroDefiner.currentHero = this;
    }

    // Update is called once per frame
    void Update()
    {

        if(CanMove){

            UpdateMoveDirection();

            if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)){
                States.Peek().Try2Jump();
            }

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)){ //面倒だし向きは移動方向と同じでいいからキーは1つでいい気がする
                States.Peek().Try2StartJet();
            }
            if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)){
                States.Peek().Try2EndJet();
            }

            if(States.Peek() != lastState){
                States.Peek().Start();
                lastState = States.Peek();
            }

            States.Peek().Update();

            MovePos(velocity.x, velocity.y);
        }
    }

    ///<summary>前フレームでの向きと今フレームの入力から移動方向を決定</summary>
    void UpdateMoveDirection(){

        if(Input.GetKeyDown(KeyCode.RightArrow) && lastDirection!=1){
            States.Peek().Try2StartMove(true);
            lastDirection = 1;

        }else if(Input.GetKeyDown(KeyCode.LeftArrow) && lastDirection!=-1){
            States.Peek().Try2StartMove(false);
            lastDirection = -1;

        }else if(Input.GetKeyUp(KeyCode.RightArrow) && lastDirection==1){

            if(Input.GetKey(KeyCode.LeftArrow)){
                States.Peek().Try2StartMove(false);
                lastDirection = -1;

            }else{
                States.Peek().Try2EndMove();
                lastDirection = 0;
            }

        }else if(Input.GetKeyUp(KeyCode.LeftArrow) && lastDirection==-1){

            if(Input.GetKey(KeyCode.RightArrow)){
                States.Peek().Try2StartMove(true);
                lastDirection = 1;

            }else{
                States.Peek().Try2EndMove();
                lastDirection = 0;
            }
        }
    }

    ///<summary>天井に衝突したときに天井に張り付かないようにする</summary>
    ///<summary>+坂道で加速させたい</summary>
    void OnCollisionStay2D(Collision2D col){

        if(CanMove){

            if(col.gameObject.tag=="Terrain"){
                foreach(ContactPoint2D contact in col.contacts){
                    if(contact.normal.y<0){
                        velocity.y = 0;
                        break;
                    }
                }
            }
        }
    }

    ///<summary>とげでOす</summary>
    void OnTriggerStay2D(Collider2D col){
        if(CanMove){
            if(col.gameObject.tag=="Toge"){
                Damage(3);
                Die();
            }
        }
    }

    //SetActive(false)するとアニメーションの状態がリセットされるようなのでとりあえず主人公はステートだけ反映しなおす
    void OnEnable(){
        if(States.Count>0) States.Peek().Start();
    }
}
