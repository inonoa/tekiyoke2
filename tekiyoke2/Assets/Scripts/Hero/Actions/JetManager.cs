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

    public void Init(IAskedInput input, HeroMover hero)
    {
        (this.input, this.hero) = (input, hero);
    }
    
    void Update()
    {
        switch(state)
        {
        case State.Inactive:
        {
            if(hero.CanMove && input.GetButtonDown(ButtonCode.JetLR))
            {
                Tokitome.SetTime(hero.Parameters.JetParams.TimeScaleBeforeJet);
                state = State.Ready;
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

            chargeSeconds += Time.deltaTime;

            if(input.GetButtonUp(ButtonCode.JetLR))
            {
                Tokitome.SetTime(1);
                state = State.Jetting;

                JetParams params_ = hero.Parameters.JetParams;
                float charge_0_1 = Mathf.Clamp01(
                    Mathf.InverseLerp
                    (
                        params_.ChargeSecondFromMin,
                        params_.ChargeSecondsToMax,
                        chargeSeconds
                    )
                );
                hero.Jet(charge_0_1)
                    .Subscribe(_ => 
                    {
                        coolTimeLeft = hero.Parameters.JetParams.CoolTime;
                        state = State.CoolTime;
                    });
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
            coolTimeLeft -= Time.deltaTime;
            if(coolTimeLeft <= 0)
            {
                state = State.Inactive;
            }
        }
        break;
        }
    }

}
