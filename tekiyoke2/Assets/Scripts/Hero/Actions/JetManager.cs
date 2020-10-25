using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class JetManager : MonoBehaviour
{
    public bool CanJet{ get; set; } = true;

    enum State{ Inactive, Ready, Jetting, CoolTime }
    State state = State.Inactive;

    float chargeSeconds = 0;
    float coolTimeLeft = 0;

    IAskedInput input;
    HeroMover hero;
    JetPostEffect jetPostEffect;
    JetCloudManager clouds;

    Subject<Unit> _JetEnded = new Subject<Unit>();
    public IObservable<Unit> JetEnded => _JetEnded;

    void Awake()
    {
        jetPostEffect = GetComponent<JetPostEffect>();
    }

    void Start()
    {
        clouds = GameUIManager.CurrentInstance.JetCloud;
    }

    public void Init(IAskedInput input, HeroMover hero)
    {
        (this.input, this.hero) = (input, hero);
        hero.OnDamaged.Subscribe(hp =>
        {
            if(state == State.Ready)
            {
                state = State.Inactive;
                chargeSeconds = 0;
                coolTimeLeft = 0;
                EffectOnExit();
            }
        });
    }
    
    void Update()
    {
        switch(state)
        {
        case State.Inactive:
        {
            if(hero.CanMove && input.GetButtonDown(ButtonCode.JetLR))
            {
                state = State.Ready;
                EffectOnReady();
            }
        }
        break;
        case State.Ready:
        {
            if(!hero.CanMove)
            {
                state = State.Inactive;
                break;
            }

            chargeSeconds += hero.TimeManager.DeltaTimeAroundHero;

            if(input.GetButtonUp(ButtonCode.JetLR))
            {
                state = State.Jetting;

                JetParams params_ = hero.Parameters.JetParams;
                float charge_0_1 = Mathf.Clamp01(
                    Mathf.InverseLerp
                    (
                        params_.ChargeSecondsFromMin * params_.TimeScaleBeforeJet,
                        params_.ChargeSecondsToMax   * params_.TimeScaleBeforeJet,
                        chargeSeconds
                    )
                );
                hero.Jet(charge_0_1)
                    .Subscribe(_ => 
                    {
                        coolTimeLeft = hero.Parameters.JetParams.CoolTime;
                        state = State.CoolTime;
                        EffectOnExit();
                        _JetEnded.OnNext(Unit.Default);
                    });

                EffectOnJet();

                chargeSeconds = 0;
            }
        }
        break;
        case State.Jetting:
        {
            //
        }
        break;
        case State.CoolTime:
        {
            coolTimeLeft -= hero.TimeManager.DeltaTimeAroundHero;
            if(coolTimeLeft <= 0)
            {
                state = State.Inactive;
            }
        }
        break;
        }
    }

    void EffectOnReady()
    {
        hero.TimeManager.SetTimeScale(hero.Parameters.JetParams.TimeScaleBeforeJet);
        jetPostEffect.Ready();
        clouds.StartClouds();
        hero.CmrCntr.StartZoomForDash();
        hero.SoundGroup.SetVolume("Tame", 0);
        hero.SoundGroup.Play("Tame");
        hero.SoundGroup.VolumeTo("Tame", 1, 0.7f);
    }

    void EffectOnJet()
    {
        hero.TimeManager.SetTimeScale(1);
        jetPostEffect.OnJet();
        clouds.EndClouds();
        hero.CmrCntr.OnJet();
        hero.SoundGroup.Play("Jet");
        hero.SoundGroup.Stop("Tame");
    }

    void EffectOnExit()
    {
        jetPostEffect.Exit();
        clouds.EndClouds();
        hero.CmrCntr.EndDash();
        hero.SoundGroup.Stop("Tame");
    }

}
