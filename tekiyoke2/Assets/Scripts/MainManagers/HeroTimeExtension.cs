using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using DG.Tweening;

public static class HeroTimeExtension
{
    public static void AsHeros(this Animator anim)
    {   
        TimeManager.Current.HeroTimeScaleRelative
            .Subscribe(timeScale =>
            {
                anim.speed = timeScale;
            })
            .AddTo(anim);
    }

    public static T AsHeros<T>(this T tween)
        where T : Tween
    {
        IDisposable subsc = TimeManager.Current.HeroTimeScaleRelative
            .Subscribe(timeScale =>
            {
                tween.timeScale = timeScale;
            });

        tween.OnComplete(() => subsc.Dispose());
        tween.OnKill(()     => subsc.Dispose());
        
        return tween;
    }
}
