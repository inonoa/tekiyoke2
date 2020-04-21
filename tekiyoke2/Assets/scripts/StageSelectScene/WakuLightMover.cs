using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakuLightMover : MonoBehaviour
{
    [SerializeField] Vector2 pathXY = new Vector2();

    enum Direction{
        Right, Down, Left, Up, None
    }

    Direction direction = Direction.Right;

    SpriteRenderer spriteRenderer;
    SpriteRenderer parentRenderer;

    public void Stop(){
        direction = Direction.None;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        spriteRenderer.color = parentRenderer.color;
        switch(direction){
            case Direction.Right:
                transform.localPosition += new Vector3(10,0,0);
                if(transform.localPosition.x>=pathXY.x){
                    transform.localPosition = new Vector3(pathXY.x,pathXY.y,0);
                    direction = Direction.Down;
                }
                break;
            case Direction.Down:
                transform.localPosition += new Vector3(0,-10,0);
                if(transform.localPosition.y<=-pathXY.y){
                    transform.localPosition = new Vector3(pathXY.x,-pathXY.y,0);
                    direction = Direction.Left;
                }
                break;
            case Direction.Left:
                transform.localPosition += new Vector3(-10,0,0);
                if(transform.localPosition.x<=-pathXY.x){
                    transform.localPosition = new Vector3(-pathXY.x,-pathXY.y,0);
                    direction = Direction.Up;
                }
                break;
            case Direction.Up:
                transform.localPosition += new Vector3(0,10,0);
                if(transform.localPosition.y>=pathXY.y){
                    transform.localPosition = new Vector3(-pathXY.x,pathXY.y,0);
                    direction = Direction.Right;
                }
                break;
        }
    }
}
