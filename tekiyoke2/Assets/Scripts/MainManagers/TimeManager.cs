using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TimeManager : MonoBehaviour
{
    ReactiveProperty<float> _TimeScaleAll        = new ReactiveProperty<float>(1);
    ReactiveProperty<float> _TimeScaleExceptHero = new ReactiveProperty<float>(1);

    public IObservable<float> TimeScaleAroundHeroObservable => _TimeScaleAll;
    public IObservable<float> TimeScaleExceptHeroObservable
        => _TimeScaleAll.CombineLatest(_TimeScaleExceptHero, Mathf.Min);

    public float TimeScaleAroundHero => _TimeScaleAll.Value;
    public float TimeScaleExceptHero => Mathf.Min(_TimeScaleAll.Value, _TimeScaleExceptHero.Value);

    public float DeltaTimeAroundHero => Time.deltaTime * TimeScaleAroundHero;
    public float DeltaTimeExceptHero => Time.deltaTime * TimeScaleExceptHero;

    public float FixedDeltaTimeAroundHero => Time.fixedDeltaTime * TimeScaleAroundHero;
    public float FixedDeltaTimeExceptHero => Time.fixedDeltaTime * TimeScaleExceptHero;

    public void SetTimeScale(float value)
    {
        //フレーム終わりにずらすべきかも
        _TimeScaleAll.Value = value;
    }
    public void SetTimeScaleExceptHero(float value)
    {
        _TimeScaleExceptHero.Value = value;
    }
    
    void Awake()
    {
        CurrentInstance = this;
    }
    public static TimeManager CurrentInstance{ get; private set; }
}
