﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : IHeroState
{
    static readonly float jumpForce = 40;
    static readonly float gravity = 2.5f;
    HeroMover hero;
    readonly bool canJump;
    public StateJump(HeroMover hero, bool canJump = true){
        this.hero = hero;
        this.canJump = canJump;
    }
    public void Try2StartJet(){
        //今のところパス
    }
    public void Try2EndJet(){ }
    public void Try2Jump(){
        if(canJump){
            hero.States.Push(new StateJump(hero, false));
        }
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

        if     (hero.velocity.x > 0) hero.anim.SetTrigger("jumprf");
        else if(hero.velocity.x < 0) hero.anim.SetTrigger("jumplf");
        else if(hero.EyeToRight)     hero.anim.SetTrigger("jumpru");
        else                         hero.anim.SetTrigger("jumplu");
    }
    public void Update(){
        hero.velocity.y -= gravity * Time.timeScale;
        if(hero.velocity.y < 0) hero.States.Push(new StateFall(hero, canJump));
    }
}
