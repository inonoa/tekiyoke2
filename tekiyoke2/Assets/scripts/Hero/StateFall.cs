using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFall : IHeroState
{
    static readonly float gravity = 2.5f;
    readonly bool canJump;

    HeroMover hero;

    public StateFall(HeroMover hero, bool canJump = true){
        this.hero = hero;
        this.canJump = canJump;
    }
    public void Try2StartJet(){
        hero.States.Push(new StateJet(hero));
    }
    public void Try2EndJet(){ }
    public void Try2Jump(){
        if(canJump){
            hero.States.Push(new StateJump(hero));
        }
    }
    public void Try2StartMove(bool toRight){
        if(toRight){
            hero.velocity.x =  HeroMover.moveSpeed;
            hero.anim.SetTrigger("fallr");
        }else{
            hero.velocity.x = -HeroMover.moveSpeed;
            hero.anim.SetTrigger("falll");
        }
    }
    public void Try2EndMove(){
        hero.velocity.x = 0;
    }
    public void Start(){
        hero.anim.SetTrigger(hero.EyeToRight ? "fallr" : "falll");
    }
    public void Update(){
        hero.velocity.y -= gravity * Time.timeScale;
        if(hero.IsOnGround){
            if(hero.velocity.x==0) hero.States.Push(new StateWait(hero));
            else hero.States.Push(new StateRun(hero));
        }
    }

    public void Exit(){ }
}
