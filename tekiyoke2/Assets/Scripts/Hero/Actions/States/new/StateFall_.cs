using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFall_ : HeroStateBase
{
    bool right = true;

    public override void Enter(HeroMover hero)
    {
        Start(hero);
    }
    public override void Resume(HeroMover hero)
    {
        Start(hero);
    }

    void Start(HeroMover hero)
    {
        right = hero.WantsToGoRight;
        hero.SetAnim("fall");
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        if(     hero.KeyDirection == 1  && !right)
        {
            right = true;
            hero.SetAnim("fall");
        }
        else if(hero.KeyDirection == -1 &&  right)
        {
            right = false;
            hero.SetAnim("fall");
        }

        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        hero.HorizontalMoveInAir(hero.Parameters.MoveInAirParams, deltatime);

        hero.ApplyGravity(hero.Parameters.MoveInAirParams, deltatime);

        if(hero.IsOnGround)
        {
            hero.SoundGroup.Play("Land");
            if(hero.KeyDirection == 0) return new StateWait_();
            else                       return new StateRun_();
        }

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        //
    }
}
