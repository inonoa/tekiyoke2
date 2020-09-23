using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWait_ : HeroStateBase
{
    float friction = 100f;

    public override void Enter(HeroMover hero)
    {
        hero.Anim.SetTrigger("standr");
    }
    public override void Resume(HeroMover hero)
    {
        hero.Anim.SetTrigger("standr");
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        if(input.GetButton(ButtonCode.Right) || input.GetButton(ButtonCode.Left))
        {
            return new StateRun_();
        }
        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        if(hero.velocity.X > 0) hero.velocity.X = Mathf.Max(hero.velocity.X - friction * deltatime, 0);
        if(hero.velocity.X < 0) hero.velocity.X = Mathf.Min(hero.velocity.X + friction * deltatime, 0);

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        //
    }
}
