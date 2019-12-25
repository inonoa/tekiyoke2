using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeroState
{
    void Start();
    void Update();
    void Try2StartJet();
    void Try2EndJet();
    void Try2Jump();
    void Try2StartMove(bool toRight);
    void Try2EndMove();
}
