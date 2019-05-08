﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HeroMover : MonoBehaviour
{

    #region 変数と関数
    ///<summary>主人公の状態。基本的にanimationの状態に対応する。(じゃあ何で作ったんだ)
    ///値が偶数だと右向き、奇数だと左向き。すごい！(適当)</summary>
    public enum HState{
        StandR,StandL,
        RunR,RunL,
        JumpR,JumpL,JumpRU,JumpLU,
        FallR,FallL,
        JetR,JetL
    }

    private HState state = HState.StandR;

    private bool IsJumping{
        get{
        if(this.State==HState.JumpL || this.State==HState.JumpLU || this.State==HState.JumpR || this.State==HState.JumpRU){
            return true;
        }
        return false;
        }
    }

    public HState State{
        get { return state;}
        set{
            if(state!=value){
                switch(value){
                    case HState.StandL:
                        anim.SetTrigger("standl");
                        break;
                    case HState.StandR:
                        anim.SetTrigger("standr");
                        break;
                    case HState.RunL:
                        anim.SetTrigger("runl");
                        break;
                    case HState.RunR:
                        anim.SetTrigger("runr");
                        break;
                    case HState.JumpL:
                        anim.SetTrigger("jumplf");
                        break;
                    case HState.JumpLU:
                        anim.SetTrigger("jumplu");
                        break;
                    case HState.JumpR:
                        anim.SetTrigger("jumprf");
                        break;
                    case HState.JumpRU:
                        anim.SetTrigger("jumpru");
                        break;
                    case HState.FallL:
                        anim.SetTrigger("falll");
                        break;
                    case HState.FallR:
                        anim.SetTrigger("fallr");
                        break;
                    case HState.JetL:
                        anim.SetTrigger("jetl");
                        break;
                    case HState.JetR:
                        anim.SetTrigger("jetr");
                        break;
                }
            }
            state = value;
        }
    }

    public static float moveSpeed = 20;
    
    ///<summary>坂道登るときちょっと本来より早くする方が気持ち良くない？そんなことない気がしてきた…</summary>
    public static float crimeBoost = 1.5f;

    ///<summary>入力に応じて-1,0,1のどれかを返す</summary>
    private int Move{
        get{
            if(Input.GetKey(KeyCode.RightArrow)){
                return 1;
            }
            else if(Input.GetKey(KeyCode.LeftArrow)){
                return -1;
            }
            else{
                return 0;
            }
        }
    }
    public static float jumpSpeed = 40;
    public static float bendBackSpeedX = 15;
    public static float bendBackSpeedY = 20;
    public float speedY = 0f;

    ///<summary>壁キック後の横方向の移動速度</summary>
    private float _SpeedX = 0;
    ///<summary>壁キック後の横方向の移動速度</summary>
    public float SpeedX{
        get{
            return _SpeedX;
        }
        set { _SpeedX = value; }
    }

    ///<summary>壁キック後の左右の速さの更新。</summary>
    void UpdateSpeedX(){
        if(IsRightFromWall){
                switch(Move){
                    case 1: _SpeedX = moveSpeed; break;
                    case 0:
                        if(_SpeedX-0.5f>0){_SpeedX -= 0.5f;}
                        else{_SpeedX = 0;}
                        break;
                    default:
                        if(_SpeedX-1>-moveSpeed){_SpeedX -= 1;}
                        else{_SpeedX = -moveSpeed;}
                        break;
                }
            }
            else{
                switch(Move){
                    case -1:_SpeedX = -moveSpeed;break;
                    case 0:
                        if(_SpeedX+0.5f<0){_SpeedX += 0.5f;}
                        else{_SpeedX = 0;}
                        break;
                    default:
                        if(_SpeedX+1<moveSpeed){_SpeedX += 1;}
                        else{_SpeedX = moveSpeed;}
                        break;
                }
            }
    }
    public static float gravity = 2.5f;
    public SpriteRenderer spriteRenderer;

    public int _JumpCount = 1;
    public int JumpCount{
        get{ return _JumpCount; }
        set{ _JumpCount = value; }
    }
    public bool isOnGround = true;
    public Animator anim;
    public Rigidbody2D rigidbody;

    ///<summary>坂道はOnCollisionStayにて管理しているためMovePositionが重複しないための措置</summary>
    private bool IsCrimbing { get; set; } = false;
    ///<summary>壁キック後の変態挙動の管理</summary>
    public bool IsFromWall { get; set; } = false;
    ///<summary>壁キックの方向。IsFromWallと併用(直したい)</summary>
    public bool IsRightFromWall { get; set; } = true;
    public bool isBendingBack = false;

    ///<summary>HPの増減はすべてここから。</summary>
    private int HP{
        get{return hpcntr.HP;}
        set{hpcntr.HP = value;}
    }
    
    ///<summary>敵からのダメージ等。ノックバックなどが入る予定</summary>
    ///<param name="damage">与えるダメージを書く。1を指定すると100->99,1->0になったりします</param>
    public void Damage(int damage){
        HP = HP - damage;
        BendBack();
    }

    ///<summary>リスポーン</summary>
    public void Die(){
        transform.position = new Vector3(0,0);
    }

    ///<summary>HPCntrからの死亡イベントをこう良い感じに…</summary>
    public void ReceiveDeath(object sender, EventArgs e){
        Die();
    }

    public HpCntr hpcntr;

    ///<summary>現状ジャンプにしてあるがそのままにしてはおけない</summary>
    public void BendBack(){
        speedY = bendBackSpeedY;
        isOnGround = false;
        isBendingBack = true;

        // 左右の入力がある場合はそれに従う
        if(Input.GetKey(KeyCode.RightArrow)){
            this.State = HState.JumpR;
            SpeedX = -bendBackSpeedX;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            this.State = HState.JumpL;
            SpeedX = bendBackSpeedX;
        }

        // 入力がない場合は前フレームの向きのまま
        else if(((int)State)%2==0){
            this.State = HState.JumpRU;
            SpeedX = -bendBackSpeedX;
        }
        else{
            this.State = HState.JumpLU;
            SpeedX = bendBackSpeedX;
        }
    }

    ///<summary>後々のためにジャンプを分離しただけ</summary>
    public void JumpOnGround(){
        Tokitome.SetTime(1);
        speedY = jumpSpeed;
        isOnGround = false;
        // 左右の入力がある場合はそれに従う
        if(Input.GetKey(KeyCode.RightArrow)){
            this.State = HState.JumpR;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            this.State = HState.JumpL;
        }
        // 入力がない場合は前フレームの向きのまま
        else if(((int)State)%2==0){
            this.State = HState.JumpRU;
        }
        else{
            this.State = HState.JumpLU;
        }
    }

    ///<summary>壁ジャンプ右、JumpCount++は苦し紛れの帳尻合わせ</summary>
    public void JumpR(){
        Tokitome.SetTime(1);
        speedY = jumpSpeed;
        isOnGround = false;
        this.State = HState.JumpR;
        this.SpeedX = moveSpeed;
        IsFromWall = true;
        IsRightFromWall = true;
        JumpCount += 1;
    }

    ///<summary>壁ジャンプ左、JumpCount++は苦し紛れの帳尻合わせ</summary>
    public void JumpL(){
        Tokitome.SetTime(1);
        speedY = jumpSpeed;
        isOnGround = false;
        this.State = HState.JumpL;
        this.SpeedX = -moveSpeed;
        IsFromWall = true;
        IsRightFromWall = false;
        JumpCount += 1;
    }

    ///<summary>空中ジャンプは回数制限があるためそのカウントを含む。</summary>
    private void JumpInSky(){
        JumpOnGround();
        JumpCount -= 1;
        IsFromWall = false;
    }

    ///<summary>指定した値だけ位置をずらす。timeScaleの影響を受けます</summary>
    public void MovePos(float vx, float vy){
        rigidbody.MovePosition(new Vector2(
            transform.position.x + vx*Time.timeScale,
            transform.position.y + vy*Time.timeScale
        ));
    }

    ///<summary>指定した値に位置が移動。timeScaleの影響を受けません</summary>
    public void WarpPos(float x, float y){
        rigidbody.MovePosition(new Vector2(x,y));
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        hpcntr.die += ReceiveDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOnGround)speedY -= gravity*Time.timeScale;

        if(isBendingBack){
            MovePos(SpeedX, speedY);
        }else{

            if(Input.GetKeyDown(KeyCode.UpArrow)){
                if (isOnGround) {
                    JumpOnGround();
                }
                else if(JumpCount > 0){
                    JumpInSky();
                }
            }

        }

        if(isOnGround){
            if(Input.GetKey(KeyCode.RightArrow)){
                State = HState.RunR;
            }
            else if(Input.GetKey(KeyCode.LeftArrow)){
                State = HState.RunL;
            }
            else if(((int)State)%2==0){
                State = HState.StandR;
            }
            else{
                State = HState.StandL;
            }
        }

        if(speedY < 0){
            if(Input.GetKey(KeyCode.RightArrow)){
                State = HState.FallR;
            }
            else if(Input.GetKey(KeyCode.LeftArrow)){
                State = HState.FallL;
            }
            else if(((int)State)%2==0){
                State = HState.FallR;
            }
            else{
                State = HState.FallL;
            }
        }

        if(!this.IsCrimbing){
            if(this.IsFromWall){
                MovePos(SpeedX, speedY);
                UpdateSpeedX();
            }else if(!isBendingBack){
                MovePos(Move * moveSpeed, speedY);
            }
            
        }

        // 落下死書かないとなー
        if(transform.position.y < -1000){
            transform.position = new Vector3(0,1000);
            speedY = 0;
        }

        this.IsCrimbing = false;
    }

    ///<summary>天井に衝突したときに天井に張り付かないようにする</summary>
    ///<summary>+坂道で加速させたい</summary>
    void OnCollisionStay2D(Collision2D col){

        if(this.IsJumping){
            foreach(ContactPoint2D contact in col.contacts){
                if(contact.normal.y<0){
                    speedY = 0;
                    break;
                }
            }
        }

        // 右向き
        if(State==HState.RunR){
            foreach(ContactPoint2D contact in col.contacts){
                if(contact.normal.x<0 & contact.normal.y!=0){
                    // 加速
                    MovePos(moveSpeed*crimeBoost, moveSpeed*crimeBoost);

                    this.IsCrimbing = true;
                    return;
                }
            }
        }

        // 左向き
        if(State==HState.RunL){
            foreach(ContactPoint2D contact in col.contacts){
                if(contact.normal.x>0 & contact.normal.y!=0){
                    // 加速
                    MovePos(-moveSpeed*crimeBoost, moveSpeed*crimeBoost);

                    this.IsCrimbing = true;
                    return;
                }
            }
        }
    }

    ///<summary>とげでOす</summary>
    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.tag=="Toge"){
            this.transform.position = new Vector2(0,0);
        }
    }
}
