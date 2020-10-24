using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBend : HeroState
{

    float secondsAfterEnter = 0;
    public override void Enter(HeroMover hero)
    {
        hero.CanMove = false;

        HeroVelocity vel = hero.Parameters.BendBackForce.ToHeroVel();
        if(!hero.WantsToGoRight) vel.X *= -1;
        hero.velocity = vel;
    }
    public override void Resume(HeroMover hero)
    {
        hero.CanMove = false;
    }

    public override HeroState HandleInput(HeroMover hero, IAskedInput input)
    {
        return this;
    }
    public override HeroState Update_(HeroMover hero, float deltatime)
    {
        hero.ApplyGravity(hero.Parameters.MoveInAirParams, deltatime);

        if(hero.IsOnGround) hero.ApplyFriction(hero.Parameters.Friction, deltatime);

        secondsAfterEnter += deltatime;
        if(secondsAfterEnter >= hero.Parameters.BendBackSeconds)
        {
            return new StateFall();
        }

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.CanMove = true;
    }
}
