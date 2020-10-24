using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTween
{
    readonly float duration;
    readonly float distance;
    readonly float linearRate;

    float now_time_0_1 = 0;

    public HeroTween(float distance, float duration, float linearRate)
    {
        (this.distance, this.duration, this.linearRate) = (distance, duration, linearRate);
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
        float quint = 1 - Mathf.Pow(1 - t, 5);
        float linear = t;
        return Mathf.Lerp(quint, linear, linearRate);
    }
}
