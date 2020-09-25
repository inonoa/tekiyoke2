using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UniRx;

public class StateJet_ : HeroStateBase
{
    float charge_0_1;
    bool needExit = false;
    Subject<Unit> _OnJetCompleted = new Subject<Unit>();
    public IObservable<Unit> OnJetCompleted => _OnJetCompleted;
    HeroTween hTween;
    public StateJet_(float charge_0_1)
    {
        this.charge_0_1 = charge_0_1;
    }
    
    public override void Enter(HeroMover hero)
    {
        hero.CanMove = false;
        hero.SetAnim("fall");

        JetParams params_ = hero.Parameters.JetParams;

        float jetSeconds = Mathf.Lerp(
            params_.JetSecondsMin,
            params_.JetSecondsMax,
            charge_0_1
        );
        float jetDistance = Mathf.Lerp(
            params_.MinDistance,
            params_.MaxDistance,
            charge_0_1
        );

        hTween = new HeroTween(
            hero.WantsToGoRight ? jetDistance : -jetDistance,
            jetSeconds,
            HeroTween.Ease.OutQuad
        );
    }

    public override void Resume(HeroMover hero)
    {
        hero.CanMove = false;
        hero.SetAnim("fall");
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        (float move, bool completed) = hTween.Update(deltatime);
        hero.velocity = new HeroVelocity(move, 0);
        if(completed)
        {
            _OnJetCompleted.OnNext(Unit.Default);
            return new StateWait_();
        }
        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.CanMove = true;
    }
}
