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

    public static IDisposable StartPausableCoroutine(this MonoBehaviour behav, IEnumerator iter)
    {
        CompositeDisposable disps = new CompositeDisposable();

        IEnumerator wrappingIter = behav.NotifyComplete(iter, () =>
        {
            disps.Dispose();
        });
        behav.StartCoroutine(wrappingIter);

        disps.Add(Pauser.Instance.OnPause.Subscribe(_ =>
        {
            behav.StopCoroutine(wrappingIter);
        }));
        disps.Add(Pauser.Instance.OnPauseEnd.Subscribe(_ =>
        {
            behav.StartCoroutine(wrappingIter);
        }));

        return disps;
    }

    static IEnumerator NotifyComplete(this MonoBehaviour behav, IEnumerator wrapped, Action onCompleted)
    {
        while(wrapped.MoveNext())
        {
            yield return wrapped.Current;
        }
        onCompleted.Invoke();
    }
}
