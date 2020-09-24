using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWait_ : HeroStateBase
{
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
        if(!hero.IsOnGround) return new StateFall_();

        hero.velocity.Y = 0;

        if(hero.velocity.X > 0) hero.velocity.X = Mathf.Max(hero.velocity.X - hero.Parameters.Friction * deltatime, 0);
        if(hero.velocity.X < 0) hero.velocity.X = Mathf.Min(hero.velocity.X + hero.Parameters.Friction * deltatime, 0);

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        //
    }
}
