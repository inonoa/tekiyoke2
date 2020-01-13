﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFall : IHeroState
{
    static readonly int inputLatency4Kick = 3;
    readonly bool canJump;
    static readonly int coyoteTime = 10;

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
        if(hero.CanKickFromWallL && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Right,ButtonCode.Jump))
            hero.States.Push(new StateKick(hero, true,  canJump));
            
        else if(hero.CanKickFromWallR && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Left,ButtonCode.Jump))
            hero.States.Push(new StateKick(hero, false, canJump));

        else if(canJump){
            if(hero.FramesSinceTakeOff < coyoteTime) hero.States.Push(new StateJump(hero, true));
            else hero.States.Push(new StateJump(hero, false));
        }
    }
    public void Try2StartMove(bool toRight){

        if(toRight){
            if(hero.CanKickFromWallL && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Right,ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, true,  canJump));

            hero.velocity.x =  HeroMover.moveSpeed;
            hero.anim.SetTrigger("fallr");

        }else{
            if(hero.CanKickFromWallR && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Left,ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, false, canJump));
                
            hero.velocity.x = -HeroMover.moveSpeed;
            hero.anim.SetTrigger("falll");
        }
    }
    public void Try2EndMove(){
        hero.velocity.x = 0;
    }
    public void Start(){
        hero.anim.SetTrigger(hero.EyeToRight ? "fallr" : "falll");
        switch(hero.KeyDirection){
            case 1 : hero.velocity.x = HeroMover.moveSpeed;  break;
            case 0 : hero.velocity.x = 0;                    break;
            case -1: hero.velocity.x = -HeroMover.moveSpeed; break;
        }
        InputManager.Instance.SetInputLatency(ButtonCode.Right,inputLatency4Kick);
        InputManager.Instance.SetInputLatency(ButtonCode.Left, inputLatency4Kick);
        InputManager.Instance.SetInputLatency(ButtonCode.Jump, inputLatency4Kick);
    }
    public void Update(){
        hero.velocity.y -= HeroMover.gravity * Time.timeScale;
        if(hero.IsOnGround){
            if(hero.KeyDirection==0) hero.States.Push(new StateWait(hero));
            else hero.States.Push(new StateRun(hero));
        }
    }

    public void Exit(){
        InputManager.Instance.SetInputLatency(ButtonCode.Right,0);
        InputManager.Instance.SetInputLatency(ButtonCode.Left,0);
        InputManager.Instance.SetInputLatency(ButtonCode.Jump,0);
    }
}
