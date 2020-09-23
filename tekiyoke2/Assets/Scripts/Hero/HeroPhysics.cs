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
}


