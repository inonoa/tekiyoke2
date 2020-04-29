using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWait : HeroState
{
    HeroMover hero;
    public StateWait(HeroMover hero){
        this.hero = hero;
    }
    public override void Try2StartJet(){
        hero.States.Push(new StateJet(hero));
    }
    public override void Try2EndJet(){ }
    public override void Try2Jump(){
        hero.States.Push(new StateJump(hero));
    }
    public override void Try2StartMove(bool toRight){
        hero.States.Push(new StateRun(hero));
        if(toRight) hero.velocity.x =  HeroMover.moveSpeed;
        else        hero.velocity.x = -HeroMover.moveSpeed;
    }
    public override void Try2EndMove(){ }
    public override void Start(){

        hero.velocity = (0,0);
        hero.anim.SetTrigger(hero.EyeToRight ? "standr" : "standl");
    }

    public override void Resume(){
        hero.anim.SetTrigger(hero.EyeToRight ? "standr" : "standl");
    }
    public override void Update(){
        if(!hero.IsOnGround){
            hero.States.Push(new StateFall(hero));
            return;
        }
        if(hero.KeyDirection != 0){
            hero.States.Push(new StateRun(hero));
            return;
        }
    }

    public override void Exit(){ }
}
