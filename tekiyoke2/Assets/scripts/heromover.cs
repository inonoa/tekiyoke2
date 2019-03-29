using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMover : MonoBehaviour
{

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
    public static float jumpSpeed = 80;
    public float speedY = 0f;
    public static float gravity = 5;
    public SpriteRenderer spriteRenderer;
    public Sprite jumpingSprite;
    public int JumpCount{
        get; set;
    } = 1;
    public bool isOnGround = true;
    public Animator anim;
    public Rigidbody2D rigidbody;


    ///<summary>後々のためにジャンプを分離しただけ</summary>
    private void JumpOnGround(){
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

    ///<summary>空中ジャンプは回数制限があるためそのカウントを含む。</summary>
    private void JumpInSky(){
        JumpOnGround();
        JumpCount -= 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOnGround)speedY -= gravity;

        if(Input.GetKeyDown(KeyCode.UpArrow)){
            if (isOnGround) {
                JumpOnGround();
            }
            else if(JumpCount > 0){
                JumpInSky();
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

        rigidbody.MovePosition(new Vector2(this.transform.position.x,this.transform.position.y)
        　　　　　　　　　　　　　+ new Vector2(Move * moveSpeed, speedY));

        if(transform.position.y < -1000){
            transform.position = new Vector3(0,1000);
            speedY = 0;
        }
    }
}
