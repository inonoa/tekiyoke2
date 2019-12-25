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
        //いまのところパス
    }
    public void Try2EndJet(){ }
    public void Try2Jump(){
        if(canJump){
            hero.States.Push(new StateJump(hero));
        }
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
    public void Update(){
        hero.velocity.y -= gravity;
    }
}
