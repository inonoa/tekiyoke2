using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using DG.Tweening;

public static class TimeScaleExtension
{
    public static void FollowTimeScale(this Animator anim, bool aroundHero)
    {
        TimeManager timeManager = TimeManager.CurrentInstance;
        IObservable<float> timeScaleObservable = aroundHero ? 
            timeManager.TimeScaleAroundHeroObservable : timeManager.TimeScaleExceptHeroObservable;
        
        timeScaleObservable.Subscribe(timeScale =>
        {
            anim.speed = timeScale;
        })
        .AddTo(anim);
    }

    public static T FollowTimeScale<T>(this T tween, bool aroundHero)
        where T : Tween
    {
        TimeManager timeManager = TimeManager.CurrentInstance;
        IObservable<float> timeScaleObservable = aroundHero ? 
            timeManager.TimeScaleAroundHeroObservable : timeManager.TimeScaleExceptHeroObservable;

        IDisposable subsc = timeScaleObservable.Subscribe(timeScale =>
        {
            tween.timeScale = timeScale;
        });

        tween.OnComplete(() => subsc.Dispose());
        tween.OnKill(()     => subsc.Dispose());
        
        return tween;
    }
}
