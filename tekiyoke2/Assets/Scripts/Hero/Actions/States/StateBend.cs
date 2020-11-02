using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StateBend : HeroState
{

    float secondsAfterEnter = 0;
    public override void Enter(HeroMover hero)
    {
        Start_(hero);

        HeroVelocity vel = hero.Parameters.BendParams.BendBackForce.ToHeroVel();
        if(!hero.WantsToGoRight) vel.X *= -1;
        hero.velocity = vel;
    }
    public override void Resume(HeroMover hero)
    {
        Start_(hero);
    }

    void Start_(HeroMover hero)
    {
        hero.CanMove = false;
        hero.SetAnim("bend");
    }

    public override HeroState HandleInput(HeroMover hero, IAskedInput input)
    {
        return this;
    }
    public override HeroState Update_(HeroMover hero, float deltatime)
    {
        var params_ = hero.Parameters;
        hero.ApplyGravity(params_.BendParams.Gravity, params_.MoveInAirParams.FallSpeedMax, deltatime);

        if(hero.IsOnGround) hero.ApplyFriction(hero.Parameters.RunParams.Friction, deltatime);

        secondsAfterEnter += deltatime;
        if(secondsAfterEnter >= hero.Parameters.BendParams.BendBackSeconds)
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
