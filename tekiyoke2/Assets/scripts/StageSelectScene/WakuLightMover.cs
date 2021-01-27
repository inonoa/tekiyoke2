using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class WakuLightMover : MonoBehaviour
{
    [SerializeField] Vector2 padding;
    
    [SerializeField] RectTransform focused;

    public void ChangeFocus(RectTransform focused, float moveDuration, Ease moveEase)
    {
        transform
            .DOMove(Target(focused), moveDuration)
            .SetEase(moveEase);
        this.focused = focused;
    }
    
    Vector2 Target(RectTransform targetRect)
    {
        Vector3[] corners = new Vector3[4];
        focused.GetWorldCorners(corners); //左下、左上、右上、右下
        
        Vector3[] targetCorners = new Vector3[4];
        targetRect.GetWorldCorners(targetCorners); //左下、左上、右上、右下 (同じ)
        
        if (direction == Direction.Right || direction == Direction.Left)
        {
            float xNormalized = Mathf.InverseLerp(corners[0].x, corners[2].x, transform.position.x);
            float targetX = Mathf.Lerp(targetCorners[0].x, targetCorners[2].x, xNormalized);
            float targetY = (direction == Direction.Right) ?
                (targetCorners[1].y - padding.y) : (targetCorners[0].y + padding.y);
            
            return new Vector2(targetX, targetY);
        }
        else
        {
            float yNormalized = Mathf.InverseLerp(corners[0].y, corners[2].y, transform.position.y);
            float targetY = Mathf.Lerp(targetCorners[0].y, targetCorners[2].y, yNormalized);
            float targetX = (direction == Direction.Up) ?
                (targetCorners[0].x - padding.x) : (targetCorners[2].x + padding.x);
            
            return new Vector2(targetX, targetY);
        }
    }

    [SerializeField] float speed = 10;

    enum Direction
    {
        Right, Down, Left, Up
    }

    [SerializeField, ReadOnly] bool moving = false;
    Direction direction = Direction.Right;

    [SerializeField] Image image;
    [SerializeField] Image wakuImage;
    public Image Image => image;

    public void Stop()
    {
        moving = false;
    }
    public void Start_()
    {
        moving = true;
    }

    void Update()
    {
        image.color = wakuImage.color;
        
        if(!moving) return;

        float move = speed * Time.deltaTime;

        switch(direction)
        {
            case Direction.Right:
                transform.localPosition += new Vector3(move, 0, 0);
                float right = focused.rect.width / 2 - padding.x;
                if(transform.localPosition.x >= right)
                {
                    transform.SetLocalX(right);
                    direction = Direction.Down;
                }
                break;
            case Direction.Down:
                transform.localPosition += new Vector3(0, -move, 0);
                float down = - focused.rect.height / 2 + padding.y;
                if(transform.localPosition.y <= down)
                {
                    transform.SetLocalY(down);
                    direction = Direction.Left;
                }
                break;
            case Direction.Left:
                transform.localPosition += new Vector3(-move, 0, 0);
                float left = - focused.rect.width / 2 + padding.x;
                if(transform.localPosition.x <= left)
                {
                    transform.SetLocalX(left);
                    direction = Direction.Up;
                }
                break;
            case Direction.Up:
                transform.localPosition += new Vector3(0, move, 0);
                float up = focused.rect.height / 2 - padding.y;
                if(transform.localPosition.y >= up)
                {
                    transform.SetLocalY(up);
                    direction = Direction.Right;
                }
                break;
        }
    }
}
