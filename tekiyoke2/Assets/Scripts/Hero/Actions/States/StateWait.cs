using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWait : HeroState
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

    public override HeroState HandleInput(HeroMover hero, IInput input)
    {
        if(input.GetButtonDown(ButtonCode.Jump))
        {
            return new StateJump();
        }
        if(input.GetButton(ButtonCode.Right) || input.GetButton(ButtonCode.Left))
        {
            return new StateRun();
        }
        return this;
    }
    public override HeroState Update_(HeroMover hero, float deltatime)
    {
        if(hero.IsOnGround)
        {
            fromNoGround = 0f;
        }
        else
        {
            fromNoGround += deltatime;
            if(fromNoGround >= hero.Parameters.RunParams.CoyoteTime) return new StateFall();
        }

        hero.velocity.Y = 0;

        hero.ApplyFriction(hero.Parameters.RunParams.Friction, deltatime);

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        //
    }
}
