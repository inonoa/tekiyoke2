using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroState
{
    public abstract void Enter(HeroMover hero);
    public abstract void Resume(HeroMover hero);

    public abstract HeroState HandleInput(HeroMover hero, IAskedInput input);
    public abstract HeroState Update_(HeroMover hero, float deltatime);

    public abstract void Exit(HeroMover hero);
}
