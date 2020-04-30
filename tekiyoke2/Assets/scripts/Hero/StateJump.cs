﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : HeroState, IAskCanJump
{
    static readonly int inputLatency4Kick = 3;
    float jumpForce;
    HeroMover hero;
    readonly bool canJump;
    public bool CanJump => canJump;
    public StateJump(HeroMover hero, bool canJump = true, float jumpForce = 30){
        this.hero = hero;
        this.canJump = canJump;
        this.jumpForce = jumpForce;
    }
    public override void Try2StartJet(){
        hero.States.Push(new StateJet(hero));
    }
    public override void Try2EndJet(){ }
    public override void Try2Jump(){
        if(hero.CanKickFromWallL && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Right,ButtonCode.Jump))
            hero.States.Push(new StateKick(hero, true,  canJump));

        else if(hero.CanKickFromWallR && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Left,ButtonCode.Jump))
            hero.States.Push(new StateKick(hero, false, canJump));

        else if(canJump) hero.States.Push(new StateJump(hero, false));
    }
    public override void Try2StartMove(bool toRight){

        if(toRight){
            if(hero.CanKickFromWallL && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Right,ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, true,  canJump));

            hero.velocity.X =  HeroMover.moveSpeed;
            hero.anim.SetTrigger("jumprf");

        }else{
            if(hero.CanKickFromWallR && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Left,ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, false, canJump));

            hero.velocity.X = -HeroMover.moveSpeed;
            hero.anim.SetTrigger("jumplf");
        }
    }
    public override void Try2EndMove(){
        hero.velocity.X = 0;
        hero.anim.SetTrigger(hero.EyeToRight ? "jumpru" : "jumplu");
    }
    public override void Start(){
        hero.velocity.Y = jumpForce;
        hero.Jumped(canJump, false);

        if     (hero.velocity.X > 0) hero.anim.SetTrigger("jumprf");
        else if(hero.velocity.X < 0) hero.anim.SetTrigger("jumplf");
        else if(hero.EyeToRight)     hero.anim.SetTrigger("jumpru");
        else                         hero.anim.SetTrigger("jumplu");

        if(canJump) hero.objsHolderForStates.JumpEffectPool.ActivateOne(hero.EyeToRight ? "r" : "l");
        else        hero.objsHolderForStates.JumpEffectInAirPool.ActivateOne(hero.EyeToRight ? "r" : "l");
        
        InputManager.Instance.SetInputLatency(ButtonCode.Right,inputLatency4Kick);
        InputManager.Instance.SetInputLatency(ButtonCode.Left, inputLatency4Kick);
        InputManager.Instance.SetInputLatency(ButtonCode.Jump, inputLatency4Kick);
    }
    public override void Resume(){
        if     (hero.velocity.X > 0) hero.anim.SetTrigger("jumprf");
        else if(hero.velocity.X < 0) hero.anim.SetTrigger("jumplf");
        else if(hero.EyeToRight)     hero.anim.SetTrigger("jumpru");
        else                         hero.anim.SetTrigger("jumplu");
    }

    public override void Update(){
        hero.velocity.Y -= HeroMover.gravity * Time.timeScale;
        if(hero.velocity.Y < 0) hero.States.Push(new StateFall(hero, canJump));
    }

    public override void Exit(){
        InputManager.Instance.SetInputLatency(ButtonCode.Right,0);
        InputManager.Instance.SetInputLatency(ButtonCode.Left,0);
        InputManager.Instance.SetInputLatency(ButtonCode.Jump,0);
    }
}
