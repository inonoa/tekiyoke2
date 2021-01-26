using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class FocusNode : MonoBehaviour
{
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

    public void Focus()
    {
        _Focused.Value = true;
    }

    public void UnFocus()
    {
        _Focused.Value = false;
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
