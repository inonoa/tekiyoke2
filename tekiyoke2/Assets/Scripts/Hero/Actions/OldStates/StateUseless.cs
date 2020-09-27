using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldStates{

public class StateUseless : HeroState
{
    public override void Start(){ }
    public override void Resume(){ }
    public override void Update(){ }
    public override void Try2StartJet(){ }
    public override void Try2EndJet(){ }
    public override void Try2Jump(){ }
    public override void Try2StartMove(bool toRight){ }
    public override void Try2EndMove(){ }
    public override void Exit(){ }
}

}
