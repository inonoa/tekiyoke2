using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeroState
{
    void Start(HeroMover hero);
    void Update(HeroMover hero);
    void Try2StartJet(HeroMover hero);
    void Try2EndJet(HeroMover hero);
    void Try2Jump(HeroMover hero);
    void Try2Move(bool toRight, HeroMover hero);
}
