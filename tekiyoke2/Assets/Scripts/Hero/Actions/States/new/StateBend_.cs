using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StateBend_ : HeroStateBase
{
    bool needExit = false;
    public override void Enter(HeroMover hero)
    {
        hero.CanMove = false;

        HeroVelocity vel = hero.Parameters.BendBackForce.ToHeroVel();
        if(!hero.WantsToGoRight) vel.X *= -1;
        hero.velocity = vel;

        DOVirtual.DelayedCall(
            hero.Parameters.BendBackSeconds,
            () => needExit = true
        );
    }
    public override void Resume(HeroMover hero)
    {
        hero.CanMove = false;
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        hero.ApplyGravity(hero.Parameters.MoveInAirParams, deltatime);

        hero.ApplyFriction(hero.Parameters.Friction, deltatime);
        
        if(needExit) return new StateFall_();
        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.CanMove = true;
    }
}
