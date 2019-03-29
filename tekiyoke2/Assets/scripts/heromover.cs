using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMover : MonoBehaviour
{
    public static float moveSpeed = 20;
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
                speedY = jumpSpeed; 
                anim.SetTrigger("jumplu");
            }
            else if(JumpCount > 0){
                speedY = jumpSpeed;
                JumpCount -= 1;
                anim.SetTrigger("jumplu");
            }
        }

        rigidbody.MovePosition(new Vector2(this.transform.position.x,this.transform.position.y)
        　　　　　　　　　　　　　+ new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, speedY));

        if(transform.position.y < -1000){
            transform.position = new Vector3(transform.position.x,1000);
            speedY = 0;
        }
    }
}
