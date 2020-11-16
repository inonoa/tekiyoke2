using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UniRx;

public class StateJet : HeroState
{
    float charge_0_1;
    Subject<Unit> _OnJetCompleted = new Subject<Unit>();
    public IObservable<Unit> OnJetCompleted => _OnJetCompleted;
    HeroTween hTween;
    public StateJet(float charge_0_1)
    {
        this.charge_0_1 = charge_0_1;
    }
    
    public override void Enter(HeroMover hero)
    {
        hero.MutekiManager.AddMutekiFilter("Jet");
        hero.CanMove = false;
        hero.SetAnim("fall");

        PhantomAndDissolve(hero);

        GameObject.Instantiate(hero.ObjsHolderForStates.JetstreamPrefab, DraftManager.CurrentInstance.GameMasterTF)
            .Init(hero);

        hero.GetDPinEnemy.GetComponent<Collider2D>().enabled = true;

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
            hero.Parameters.JetParams.TweenLinearRate
        );
    }

    public override void Resume(HeroMover hero)
    {
        hero.CanMove = false;
        hero.SetAnim("fall");
    }

    public override HeroState HandleInput(HeroMover hero, IAskedInput input)
    {
        return this;
    }
    public override HeroState Update_(HeroMover hero, float deltatime)
    {
        (float move, bool completed) = hTween.Update(deltatime);
        hero.velocity = new HeroVelocity(move, 0);
        if(completed)
        {
            if(hero.IsOnGround) return new StateWait();
            else                return new StateFall();
        }
        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.MutekiManager.RemoveMutekiFilter("Jet");
        hero.CanMove = true;
        hero.GetDPinEnemy.GetComponent<Collider2D>().enabled = false;
        _OnJetCompleted.OnNext(Unit.Default);
    }

    void PhantomAndDissolve(HeroMover hero)
    {
        SpriteRenderer phantom = hero.ObjsHolderForStates.PhantomRenderer;
        phantom.gameObject.SetActive(true);
        phantom.transform.position = hero.transform.position;
        phantom.sprite = hero.SpriteRenderer.sprite;

        Material heroMat = hero.SpriteRenderer.material;
        Material phantomMat = phantom.material;

        heroMat.SetFloat("_DisThreshold0", 1);
        heroMat.SetFloat("_DisThreshold1", 1.1f);
        phantomMat.SetFloat("_DisThreshold0", -1);
        phantomMat.SetFloat("_DisThreshold1", 0);

        DOVirtual.DelayedCall(0.2f, () =>
        {
            float heroAppearSec = 0.3f;
            heroMat.To("_DisThreshold0", -1, heroAppearSec);
            heroMat.To("_DisThreshold1", 0,  heroAppearSec);
        }, ignoreTimeScale: false)
        .AsHeros();

        float phantomDisappearSec = 0.3f;
        phantomMat.To("_DisThreshold0", 1,    phantomDisappearSec).AsHeros();
        phantomMat.To("_DisThreshold1", 1.1f, phantomDisappearSec).AsHeros()
            .onComplete = () => phantom.gameObject.SetActive(false);
    }
}
