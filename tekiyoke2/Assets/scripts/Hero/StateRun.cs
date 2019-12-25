using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : IHeroState
{
    HeroMover hero;
    public StateRun(HeroMover hero){
        this.hero = hero;
    }
    public void Try2StartJet(){
        //今のところパス
    }
    public void Try2EndJet(){ }
    public void Try2Jump(){
        hero.States.Push(new StateJump(hero));
    }
    public void Try2StartMove(bool toRight){
        if(toRight) hero.velocity.x =  HeroMover.moveSpeed;
        else        hero.velocity.x = -HeroMover.moveSpeed;
    }
    public void Try2EndMove(){
        hero.velocity.x = 0;
    }
    public void Start(){

    }
    public void Update(){ }
}
