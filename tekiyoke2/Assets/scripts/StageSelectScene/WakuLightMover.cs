using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakuLightMover : MonoBehaviour
{
    [SerializeField] Vector2 pathXY = new Vector2();

    [SerializeField] float speed = 10;

    enum Direction
    {
        Right, Down, Left, Up, None
    }

    Direction direction = Direction.Right;

    [SerializeField] Image image;
    [SerializeField] Image wakuImage;

    public void Stop()
    {
        direction = Direction.None;
    }

    void Update()
    {
        image.color = wakuImage.color;

        float move = speed * Time.deltaTime;
        
        switch(direction)
        {
            case Direction.Right:
                transform.localPosition += new Vector3(move, 0, 0);
                if(transform.localPosition.x >= pathXY.x)
                {
                    transform.localPosition = new Vector3(pathXY.x, pathXY.y, 0);
                    direction = Direction.Down;
                }
                break;
            case Direction.Down:
                transform.localPosition += new Vector3(0, -move, 0);
                if(transform.localPosition.y <= -pathXY.y)
                {
                    transform.localPosition = new Vector3(pathXY.x, -pathXY.y, 0);
                    direction = Direction.Left;
                }
                break;
            case Direction.Left:
                transform.localPosition += new Vector3(-move, 0, 0);
                if(transform.localPosition.x <= -pathXY.x)
                {
                    transform.localPosition = new Vector3(-pathXY.x, -pathXY.y, 0);
                    direction = Direction.Up;
                }
                break;
            case Direction.Up:
                transform.localPosition += new Vector3(0, move, 0);
                if(transform.localPosition.y >= pathXY.y)
                {
                    transform.localPosition = new Vector3(-pathXY.x, pathXY.y, 0);
                    direction = Direction.Right;
                }
                break;
        }
    }
}
