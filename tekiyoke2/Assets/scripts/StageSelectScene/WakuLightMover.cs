using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakuLightMover : MonoBehaviour
{
    enum Direction{
        Right, Down, Left, Up, None
    }

    Direction direction = Direction.Right;

    SpriteRenderer spriteRenderer;
    SpriteRenderer parentRenderer;

    public void Stop(){
        direction = Direction.None;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        spriteRenderer.color = parentRenderer.color;
        switch(direction){
            case Direction.Right:
                transform.localPosition += new Vector3(10,0,0);
                if(transform.localPosition.x>=250){
                    transform.localPosition = new Vector3(250,75,0);
                    direction = Direction.Down;
                }
                break;
            case Direction.Down:
                transform.localPosition += new Vector3(0,-10,0);
                if(transform.localPosition.y<=-75){
                    transform.localPosition = new Vector3(250,-75,0);
                    direction = Direction.Left;
                }
                break;
            case Direction.Left:
                transform.localPosition += new Vector3(-10,0,0);
                if(transform.localPosition.x<=-250){
                    transform.localPosition = new Vector3(-250,-75,0);
                    direction = Direction.Up;
                }
                break;
            case Direction.Up:
                transform.localPosition += new Vector3(0,10,0);
                if(transform.localPosition.y>=75){
                    transform.localPosition = new Vector3(-250,75,0);
                    direction = Direction.Right;
                }
                break;
        }
    }
}
