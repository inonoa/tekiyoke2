using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class JetManager : MonoBehaviour
{
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

    void Start()
    {
        
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
                state = State.Jetting;
                hero.Jet(CalcJetDistance(chargeSeconds, hero.Parameters.JetParams))
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

    float CalcJetDistance(float chargeSecs, JetParams params_)
    {
        float chargeSecsNormalized = Mathf.InverseLerp
        (
            params_.ChargeSecondFromMin,
            params_.ChargeSecondsToMax,
            chargeSeconds
        );
        return Mathf.Lerp(params_.MinDistance, params_.MaxDistance, chargeSecsNormalized);
    }

}
