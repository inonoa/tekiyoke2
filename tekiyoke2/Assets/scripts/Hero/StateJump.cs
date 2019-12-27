using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : IHeroState
{
    static readonly float jumpForce = 40;
    HeroMover hero;
    readonly bool canJump;
    public StateJump(HeroMover hero, bool canJump = true){
        this.hero = hero;
        this.canJump = canJump;
    }
    public void Try2StartJet(){
        hero.States.Push(new StateJet(hero));
    }
    public void Try2EndJet(){ }
    public void Try2Jump(){
        if(hero.CanKickFromWallL)      hero.States.Push(new StateKick(hero, true,  canJump));
        else if(hero.CanKickFromWallR) hero.States.Push(new StateKick(hero, false, canJump));

        else if(canJump) hero.States.Push(new StateJump(hero, false));
    }
    public void Try2StartMove(bool toRight){
        if(toRight){
            hero.velocity.x =  HeroMover.moveSpeed;
            hero.anim.SetTrigger("jumprf");
        }else{
            hero.velocity.x = -HeroMover.moveSpeed;
            hero.anim.SetTrigger("jumplf");
        }
    }
    public void Try2EndMove(){
        hero.velocity.x = 0;
        hero.anim.SetTrigger(hero.EyeToRight ? "jumpru" : "jumplu");
    }
    public void Start(){
        hero.velocity.y = StateJump.jumpForce;
        hero.Jumped();

        if     (hero.velocity.x > 0) hero.anim.SetTrigger("jumprf");
        else if(hero.velocity.x < 0) hero.anim.SetTrigger("jumplf");
        else if(hero.EyeToRight)     hero.anim.SetTrigger("jumpru");
        else                         hero.anim.SetTrigger("jumplu");
    }
    public void Update(){
        hero.velocity.y -= HeroMover.gravity * Time.timeScale;
        if(hero.velocity.y < 0) hero.States.Push(new StateFall(hero, canJump));
    }

    public void Exit(){ }
}
