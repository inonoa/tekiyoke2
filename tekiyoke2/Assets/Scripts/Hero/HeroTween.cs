using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeroTween
{
    public enum Ease{ OutQuint, OutQuad }
    Ease ease;
    float duration;
    float distance;

    float now_time_0_1 = 0;

    public HeroTween(float distance, float duration, Ease ease)
    {
        (this.distance, this.duration, this.ease) = (distance, duration, ease);
    }

    public (float move, bool completed) Update(float deltatime)
    {
        float next_time_0_1 = Mathf.Clamp01(now_time_0_1 + deltatime / duration);

        float move = (Calc(next_time_0_1) - Calc(now_time_0_1)) * distance;

        now_time_0_1 = next_time_0_1;

        return (move, now_time_0_1 == 1);
    }

    float Calc(float t)
    {
        switch(ease)
        {
        case Ease.OutQuint:
            return 1 - Mathf.Pow(1 - t, 5);
        case Ease.OutQuad:
            return 1 - (1 - t) * (1 - t);
        default:
            return 0;
        }
    }
}
