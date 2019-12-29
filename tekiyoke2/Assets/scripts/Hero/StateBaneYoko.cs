using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBaneYoko : IHeroState
{
    readonly bool toRight;
    readonly float pushSpeed;
    readonly HeroMover hero;

    public StateBaneYoko(HeroMover hero, bool toRight, float pushSpeed = 100){
        this.hero = hero;
        this.toRight = toRight;
        this.pushSpeed = pushSpeed;
    }

    public void Start(){
        hero.velocity.x = toRight ? pushSpeed : -pushSpeed;
        hero.anim.SetTrigger(toRight ? "runr" : "runl");
    }

    public void Update(){

    }
    
    public void Try2StartJet(){ }
    public void Try2EndJet(){ }
    public void Try2Jump(){ }
    public void Try2StartMove(bool toRight){ }
    public void Try2EndMove(){ }
    public void Exit(){ }
}
