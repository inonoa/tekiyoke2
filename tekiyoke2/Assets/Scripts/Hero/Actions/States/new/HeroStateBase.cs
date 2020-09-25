using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroStateBase
{
    public abstract void Enter(HeroMover hero);
    public abstract void Resume(HeroMover hero);

    public abstract HeroStateBase HandleInput(HeroMover hero, IAskedInput input);
    public abstract HeroStateBase Update_(HeroMover hero, float deltatime);

    public abstract void Exit(HeroMover hero);
}
