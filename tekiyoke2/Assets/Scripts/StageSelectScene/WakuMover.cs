using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WakuMover : MonoBehaviour
{
    [SerializeField] Vector2 padding;
    [SerializeField] float speed = 10;
    
    [FormerlySerializedAs("image")] [SerializeField] Image lightImage;
    [SerializeField] Image wakuImage;
    [SerializeField] RectTransform focused;
    public Image LightImage => lightImage;
    public Image WakuImage => wakuImage;

    new public Transform transform => base.transform;

    public void ChangeFocus(RectTransform focused)
    {
        float moveDuration = 0.3f;
        Ease moveEase = Ease.OutCubic;
        
        WakuImage.rectTransform.DOSizeDelta(focused.sizeDelta, moveDuration).SetEase(moveEase);
        WakuImage.rectTransform.DOMove(focused.position, moveDuration).SetEase(moveEase);
        
        lightImage.transform
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
            float xNormalized = Mathf.InverseLerp(corners[0].x, corners[2].x, lightImage.transform.position.x);
            float targetX = Mathf.Lerp(targetCorners[0].x, targetCorners[2].x, xNormalized);
            float targetY = (direction == Direction.Right) ?
                (targetCorners[1].y - padding.y) : (targetCorners[0].y + padding.y);
            
            return new Vector2(targetX, targetY);
        }
        else
        {
            float yNormalized = Mathf.InverseLerp(corners[0].y, corners[2].y, lightImage.transform.position.y);
            float targetY = Mathf.Lerp(targetCorners[0].y, targetCorners[2].y, yNormalized);
            float targetX = (direction == Direction.Up) ?
                (targetCorners[0].x - padding.x) : (targetCorners[2].x + padding.x);
            
            return new Vector2(targetX, targetY);
        }
    }

    enum Direction
    {
        Right, Down, Left, Up
    }

    [SerializeField, ReadOnly] bool moving = false;
    Direction direction = Direction.Right;

    public void Stop()
    {
        moving = false;
    }
    public void Start_()
    {
        moving = true;
    }

    public void FadeIn(float duration, Ease ease)
    {
        wakuImage.DOFade(1, duration).SetEase(ease);
        lightImage.DOFade(1, duration).SetEase(ease);
    }

    public void FadeOut(float duration, Ease ease)
    {
        wakuImage.DOFade(0, duration).SetEase(ease);
        lightImage.DOFade(0, duration).SetEase(ease);
    }

    public void SetInvisible()
    {
        wakuImage.SetAlpha(0);
        lightImage.SetAlpha(0);
    }

    void Update()
    {
        lightImage.color = wakuImage.color;
        
        if(!moving) return;

        float move = speed * Time.deltaTime;

        switch(direction)
        {
            case Direction.Right:
                lightImage.transform.localPosition += new Vector3(move, 0, 0);
                float right = focused.rect.width / 2 - padding.x;
                if(lightImage.transform.localPosition.x >= right)
                {
                    lightImage.transform.SetLocalX(right);
                    direction = Direction.Down;
                }
                break;
            case Direction.Down:
                lightImage.transform.localPosition += new Vector3(0, -move, 0);
                float down = - focused.rect.height / 2 + padding.y;
                if(lightImage.transform.localPosition.y <= down)
                {
                    lightImage.transform.SetLocalY(down);
                    direction = Direction.Left;
                }
                break;
            case Direction.Left:
                lightImage.transform.localPosition += new Vector3(-move, 0, 0);
                float left = - focused.rect.width / 2 + padding.x;
                if(lightImage.transform.localPosition.x <= left)
                {
                    lightImage.transform.SetLocalX(left);
                    direction = Direction.Up;
                }
                break;
            case Direction.Up:
                lightImage.transform.localPosition += new Vector3(0, move, 0);
                float up = focused.rect.height / 2 - padding.y;
                if(lightImage.transform.localPosition.y >= up)
                {
                    lightImage.transform.SetLocalY(up);
                    direction = Direction.Right;
                }
                break;
        }
    }
}
