using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heromover : MonoBehaviour
{
    public static float moveSpeed = 0.2f;
    public static float jumpSpeed = 0.8f;
    public float speedY = 0f;
    public static float gravity = 0.05f;
    public static float groundHeight = -1.5f;
    public SpriteRenderer spriteRenderer;
    public Sprite jumpingSprite;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 接地判定
        if(this.transform.position.y <= groundHeight){
            this.transform.position = new Vector3(this.transform.position.x,groundHeight);
            if(speedY < 0){
                speedY = 0;
            }
        }

        this.transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, 0);
        this.transform.position += new Vector3(0,speedY);
        speedY -= gravity;

        if(Input.GetKeyDown(KeyCode.UpArrow) && this.transform.position.y <= groundHeight){
            speedY = jumpSpeed;
            spriteRenderer.sprite = jumpingSprite;
        }
    }
}
