using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUseless : IHeroState
{
    public void Start(){ }
    public void Update(){ }
    public void Try2StartJet(){ }
    public void Try2EndJet(){ }
    public void Try2Jump(){ }
    public void Try2StartMove(bool toRight){ }
    public void Try2EndMove(){ }
    public void Exit(){ }
}
