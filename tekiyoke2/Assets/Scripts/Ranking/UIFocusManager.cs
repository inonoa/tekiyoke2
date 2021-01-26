using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIFocusManager : SerializedMonoBehaviour
{
    [SerializeField] FocusNode initialNode;

    [SerializeField, ReadOnly] FocusNode focused;
    public FocusNode Focused => focused;

    IAskedInput input;
    
    public void OnExit()
    {
        //
    }

    public void OnEnter()
    {
        //
    }

    void Start()
    {
        input = InputManager.Instance; //うーんこの
        
        focused = initialNode;
        initialNode.Focus();
    }

    void Update()
    {
        if (input.GetButtonDown(ButtonCode.Left))
        {
            if (focused.Left != null)
            {
                focused.UnFocus();
                focused = focused.Left;
                focused.Focus();
            }
        }
        if (input.GetButtonDown(ButtonCode.Right))
        {
            if (focused.Right != null)
            {
                focused.UnFocus();
                focused = focused.Right;
                focused.Focus();
            }
        }
        if (input.GetButtonDown(ButtonCode.Up))
        {
            if (focused.Up != null)
            {
                focused.UnFocus();
                focused = focused.Up;
                focused.Focus();
            }
        }
        if (input.GetButtonDown(ButtonCode.Down))
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
