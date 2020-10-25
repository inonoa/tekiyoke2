using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public static class TimeScaleExtension
{
    public static void FollowTimeScale(this Animator anim, bool aroundhero)
    {
        TimeManager timeManager = TimeManager.CurrentInstance;
        IObservable<float> timeScaleObservable = aroundhero ? 
            timeManager.TimeScaleAroundHeroObservable : timeManager.TimeScaleExceptHeroObservable;
        
        timeScaleObservable.Subscribe(timeScale =>
        {
            anim.speed = timeScale;
        });
    }
}
