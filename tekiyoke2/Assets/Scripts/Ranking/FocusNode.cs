using System;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class FocusNode : SerializedMonoBehaviour
{
    [SerializeField] IFocusManager manager;
    public IFocusManager Manager => manager;

    [SerializeField, ReadOnly]
    BoolReactiveProperty _Focused = new BoolReactiveProperty(false);
    public bool Focused => _Focused.Value;

    public IObservable<Unit> OnFocused
        => _Focused
            .SkipLatestValueOnSubscribe()
            .Where(focused => focused)
            .Select(_ => Unit.Default);

    public IObservable<Unit> OnUnFocused
        => _Focused
            .SkipLatestValueOnSubscribe()
            .Where(focused => !focused)
            .Select(_ => Unit.Default);

    public IObservable<Unit> OnSelected { get; private set; }

    void Awake()
    {
        OnSelected = this.UpdateAsObservable()
            .Where(_ => manager.SelectButtonDown())
            .Where(_ => manager.AcceptsInput)
            .Where(_ => this.Focused);
    }

    public void Focus()
    {
        _Focused.Value = true;
    }

    public void UnFocus()
    {
        _Focused.Value = false;
    }

    [field: SerializeField, LabelText(nameof(Left))]
    public FocusNode Left { get; set; }
    
    [field: SerializeField, LabelText(nameof(Right))]
    public FocusNode Right { get; set; }
    
    [field: SerializeField, LabelText(nameof(Up))]
    public FocusNode Up { get; set; }
    
    [field: SerializeField, LabelText(nameof(Down))]
    public FocusNode Down { get; set; }
}

public interface IFocusManager
{
    bool AcceptsInput { get; }
    bool SelectButtonDown();
}
