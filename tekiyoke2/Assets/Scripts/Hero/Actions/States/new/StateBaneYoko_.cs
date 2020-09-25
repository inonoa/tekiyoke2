using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBaneYoko_ : HeroStateBase
{
    
    public override void Enter(HeroMover hero)
    {
        //
    }
    public override void Resume(HeroMover hero)
    {
        //
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        return this;
    }

    public override void Exit(HeroMover hero)
    {
        //
    }
}
