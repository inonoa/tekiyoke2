using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class UIFocusManager : SerializedMonoBehaviour, IFocusManager
{
    [SerializeField] FocusNode initialNode;

    [SerializeField, ReadOnly] FocusNode focused;
    public FocusNode Focused => focused;


    Subject<FocusNode> _OnNodeFocused = new Subject<FocusNode>();
    public IObservable<FocusNode> OnNodeFocused => _OnNodeFocused;

    [SerializeField] IAskedInput input;
    
    public void OnExit()
    {
        AcceptsInput = false;
    }

    public void OnEnter()
    {
        AcceptsInput = true;
    }

    void Start()
    {
        focused = initialNode;
        initialNode.Focus();
        _OnNodeFocused.OnNext(initialNode);
    }

    public bool AcceptsInput { get; private set; } = true;
    public bool SelectButtonDown() => input.GetButtonDown(ButtonCode.Enter);

    void Update()
    {
        if(!AcceptsInput) return;
        
        if (input.GetButtonDown(ButtonCode.Left))
        {
            if (focused.Left != null)
            {
                focused.UnFocus();
                focused = focused.Left;
                focused.Focus();
                _OnNodeFocused.OnNext(focused);
            }
        }
        if (input.GetButtonDown(ButtonCode.Right))
        {
            if (focused.Right != null)
            {
                focused.UnFocus();
                focused = focused.Right;
                focused.Focus();
                _OnNodeFocused.OnNext(focused);
            }
        }
        if (input.GetButtonDown(ButtonCode.Up))
        {
            if (focused.Up != null)
            {
                focused.UnFocus();
                focused = focused.Up;
                focused.Focus();
                _OnNodeFocused.OnNext(focused);
            }
        }
        if (input.GetButtonDown(ButtonCode.Down))
        {
            if (focused.Down != null)
            {
                focused.UnFocus();
                focused = focused.Down;
                focused.Focus();
                _OnNodeFocused.OnNext(focused);
            }
        }
    }
}
