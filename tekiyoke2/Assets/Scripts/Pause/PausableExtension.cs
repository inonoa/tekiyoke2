using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using DG.Tweening;

public static class PausableExtension
{
    public static IDisposable GetPausable(this Tween tween)
    {
        CompositeDisposable disps = new CompositeDisposable();

        disps.Add(Pauser.Instance.OnPause
            .Subscribe(_ => tween?.Pause()));
        disps.Add(Pauser.Instance.OnPauseEnd
            .Subscribe(_ => tween?.TogglePause()));

        tween.OnComplete(() =>
        {
            disps.Dispose();
        });
        tween.OnKill(() =>
        {
            disps.Dispose();
        });

        return disps;
    }
}
