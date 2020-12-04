using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class FocusNode : MonoBehaviour
{
    public IObservable<Unit> OnFocused => _OnFocused;
    Subject<Unit> _OnFocused = new Subject<Unit>();
    
    public IObservable<Unit> OnUnFocused => _OnUnFocused;
    Subject<Unit> _OnUnFocused = new Subject<Unit>();

    [field: SerializeField, ReadOnly, LabelText(nameof(Focused))]
    public bool Focused { get; private set; } = false;

    public void Focus()
    {
        Focused = true;
        _OnFocused.OnNext(Unit.Default);
    }

    public void UnFocus()
    {
        Focused = false;
        _OnUnFocused.OnNext(Unit.Default);
    }

    [field: SerializeField, LabelText(nameof(Left))]
    public FocusNode Left { get; private set; }
    
    [field: SerializeField, LabelText(nameof(Right))]
    public FocusNode Right { get; private set; }
    
    [field: SerializeField, LabelText(nameof(Up))]
    public FocusNode Up { get; private set; }
    
    [field: SerializeField, LabelText(nameof(Down))]
    public FocusNode Down { get; private set; }
}
