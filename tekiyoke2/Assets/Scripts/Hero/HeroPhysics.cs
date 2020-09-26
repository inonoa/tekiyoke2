using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeroPhysics
{
    public static void HorizontalMoveInAir(this HeroMover hero, MoveInAirParams params_, float deltatime)
    {
        switch(hero.KeyDirection)
        {
        case 1:
            hero.velocity.X = Mathf.Min(
                hero.velocity.X + params_.HorizontalForce * deltatime,
                params_.HorizontalSpeedMax);
            break;

        case -1:
            hero.velocity.X = Mathf.Max(
                hero.velocity.X - params_.HorizontalForce * deltatime,
                -params_.HorizontalSpeedMax);
            break;

        case 0:
            if(hero.velocity.X > 0)
            {
                hero.velocity.X = Mathf.Max(
                    hero.velocity.X - params_.HorizontalResistance * deltatime,
                    0);
            }
            else
            {
                hero.velocity.X = Mathf.Min(
                    hero.velocity.X + params_.HorizontalResistance * deltatime,
                    0);
            }
            break;
        }
    }

    public static void ApplyGravity(this HeroMover hero, MoveInAirParams params_, float deltatime)
    {
        hero.velocity.Y = Mathf.Max(
            hero.velocity.Y - params_.Gravity * deltatime,
            -params_.FallSpeedMax);
    }

    public static void ApplyFriction(this HeroMover hero, float friction, float deltatime)
    {
        if(!hero.IsOnGround) return;

        if(hero.velocity.X > 0) hero.velocity.X = Mathf.Max(hero.velocity.X - friction * deltatime, 0);
        if(hero.velocity.X < 0) hero.velocity.X = Mathf.Min(hero.velocity.X + friction * deltatime, 0);
    }

    public static void ApplySakamichi(this HeroMover hero)
    {
        float absVx = Mathf.Abs(hero.velocity.X);
        if(hero.IsOnSakamichiL)
        {
            hero.velocity.Y = hero.WantsToGoRight ? -absVx :  absVx;
        }
        else if(hero.IsOnSakamichiR)
        {
            hero.velocity.Y = hero.WantsToGoRight ?  absVx : -absVx;
        }
    }
}


