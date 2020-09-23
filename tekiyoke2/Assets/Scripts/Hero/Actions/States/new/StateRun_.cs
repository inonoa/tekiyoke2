﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun_ : HeroStateBase
{
    float speed = 15f;
    float runForce = 150f;

    bool right = true;

    public override void Enter(HeroMover hero)
    {
        hero.SetAnim("run");
        right = hero.WantsToGoRight;
    }
    public override void Resume(HeroMover hero)
    {
        hero.SetAnim("run");
        right = hero.WantsToGoRight;
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        if(input.GetButtonDown(ButtonCode.Jump))
        {
            return new StateJump_();
        }
        if(! (input.GetButton(ButtonCode.Right) || input.GetButton(ButtonCode.Left)))
        {
            return new StateWait_();
        }

        if(hero.KeyDirection == 1  && !right)
        {
            right = true;
            hero.SetAnim("run");
        }
        if(hero.KeyDirection == -1 &&  right)
        {
            right = false;
            hero.SetAnim("run");
        }

        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        if(hero.KeyDirection == 0)
        {
            return new StateWait_();
        }

        if(     hero.KeyDirection == 1)
        {
            hero.velocity.X = Mathf.Min(hero.velocity.X + runForce * deltatime,  speed);
        }
        else if(hero.KeyDirection == -1)
        {
            hero.velocity.X = Mathf.Max(hero.velocity.X - runForce * deltatime, -speed);
        }
        return this;
    }

    public override void Exit(HeroMover hero)
    {
        //
    }
}