using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class TimeManager : MonoBehaviour
{
    float[] effects = Enumerable.Repeat
                      (
                          1f,
                          Enum.GetValues(typeof(TimeEffectType)).Length
                      )
                      .ToArray();
    float effectExceptHero = 1;

    ReactiveProperty<float> _HeroTimeScaleRelative = new ReactiveProperty<float>(1);
    public IObservable<float> HeroTimeScaleRelative => _HeroTimeScaleRelative;

    public void SetTimeScale(TimeEffectType type, float value)
    {
        effects[(int)type] = value;
        ReCalcurateTimeScales();
    }
    public void SetTimeScaleExceptHero(float value)
    {
        effectExceptHero = value;
        ReCalcurateTimeScales();
    }

    void ReCalcurateTimeScales()
    {
        Time.timeScale = effects.Min() * effectExceptHero;
        if(Time.timeScale != 0) Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

        _HeroTimeScaleRelative.Value = 1 / effectExceptHero;
    }

    public void Reset()
    {
        effects = Enumerable.Repeat
                  (
                      1f,
                      Enum.GetValues(typeof(TimeEffectType)).Length
                  )
                  .ToArray();
        effectExceptHero = 1;

        ReCalcurateTimeScales();
    }

    public float TimeScaleExceptHero => Time.timeScale;
    public float TimeScaleAroundHero => Time.timeScale / effectExceptHero;
    public float DeltaTimeExceptHero => Time.deltaTime;
    public float DeltaTimeAroundHero => Time.deltaTime / effectExceptHero;
    public float FixedDeltaTimeExceptHero => Time.fixedDeltaTime;
    public float FixedDeltaTimeAroundHero => Time.fixedDeltaTime / effectExceptHero;

    public float TimeExceptHero{ get; private set; } = 0;
    public float TimeAroundHero{ get; private set; } = 0;

    void Update()
    {
        TimeAroundHero += DeltaTimeAroundHero;
        TimeExceptHero += DeltaTimeExceptHero;
    }

    void OnDestroy()
    {
        CurrentInstance = null;
    }

    readonly float defaultFixedDeltaTime = 0.02f;
    void Awake()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
        CurrentInstance = this;
    }
    public static TimeManager CurrentInstance{ get; private set; }
}

public enum TimeEffectType
{
    Die, ReadyToJet, GetDP
}
