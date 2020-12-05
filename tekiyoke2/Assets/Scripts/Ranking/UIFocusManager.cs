using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIFocusManager : SerializedMonoBehaviour
{
    [SerializeField] FocusNode initialNode;

    [SerializeField, ReadOnly] FocusNode focused;
    
    public void OnExit()
    {
        focused.UnFocus();
        focused = null;
    }

    public void OnEnter()
    {
        focused = initialNode;
        initialNode.Focus();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (focused.Left != null)
            {
                focused.UnFocus();
                focused = focused.Left;
                focused.Focus();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (focused.Right != null)
            {
                focused.UnFocus();
                focused = focused.Right;
                focused.Focus();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (focused.Up != null)
            {
                focused.UnFocus();
                focused = focused.Up;
                focused.Focus();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (focused.Down != null)
            {
                focused.UnFocus();
                focused = focused.Down;
                focused.Focus();
            }
        }
    }
}
