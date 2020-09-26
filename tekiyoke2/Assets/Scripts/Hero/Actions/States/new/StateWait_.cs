using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWait_ : HeroStateBase
{
    float fromNoGround = 0f;

    public override void Enter(HeroMover hero)
    {
        hero.SetAnim("stand");
    }
    public override void Resume(HeroMover hero)
    {
        hero.SetAnim("stand");
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        if(input.GetButtonDown(ButtonCode.Jump))
        {
            return new StateJump_();
        }
        if(input.GetButton(ButtonCode.Right) || input.GetButton(ButtonCode.Left))
        {
            return new StateRun_();
        }
        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        if(hero.IsOnGround)
        {
            fromNoGround = 0f;
        }
        else
        {
            fromNoGround += deltatime;
            if(fromNoGround >= hero.Parameters.CoyoteTime) return new StateFall_();
        }

        hero.velocity.Y = 0;

        hero.ApplyFriction(hero.Parameters.Friction, deltatime);

        hero.ApplySakamichi();

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        //
    }
}
