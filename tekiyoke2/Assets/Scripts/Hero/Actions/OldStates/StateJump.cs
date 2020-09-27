﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldStates{

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
        IAskedInput input = hero.Input;

        if(hero.CanKickFromWallL && input.GetButton(ButtonCode.Left) && input.GetButtonDown(ButtonCode.Jump)){
            hero.States.Push(new StateKick(hero, true,  canJump));
            
        }else if(hero.CanKickFromWallR && input.GetButton(ButtonCode.Right) && input.GetButtonDown(ButtonCode.Jump)){
            hero.States.Push(new StateKick(hero, false, canJump));

        }else if(canJump) hero.States.Push(new StateJump(hero, false));
    }
    public override void Try2StartMove(bool toRight){
        IAskedInput input = hero.Input;

        if(toRight){
            if(hero.CanKickFromWallR && input.GetButton(ButtonCode.Right) && input.GetButtonDown(ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, false, canJump));

            hero.velocity.X =  HeroMover.moveSpeed;
            hero.Anim.SetTrigger("jumprf");

        }else{
            if(hero.CanKickFromWallL && input.GetButton(ButtonCode.Left) && input.GetButtonDown(ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, true,  canJump));

            hero.velocity.X = -HeroMover.moveSpeed;
            hero.Anim.SetTrigger("jumplf");
        }
    }
    public override void Try2EndMove(){
        hero.velocity.X = 0;
        hero.Anim.SetTrigger(hero.WantsToGoRight ? "jumpru" : "jumplu");
    }
    public override void Start(){
        hero.velocity.Y = jumpForce;
        hero.Jumped(canJump, false);

        if     (hero.velocity.X > 0) hero.Anim.SetTrigger("jumprf");
        else if(hero.velocity.X < 0) hero.Anim.SetTrigger("jumplf");
        else if(hero.WantsToGoRight)     hero.Anim.SetTrigger("jumpru");
        else                         hero.Anim.SetTrigger("jumplu");

        if(canJump) hero.ObjsHolderForStates.JumpEffectPool.ActivateOne(hero.WantsToGoRight ? "r" : "l");
        else        hero.ObjsHolderForStates.JumpEffectInAirPool.ActivateOne(hero.WantsToGoRight ? "r" : "l");

        hero.SoundGroup.Play("Jump");
        
        hero.Input.SetInputLatency(ButtonCode.Right,inputLatency4Kick);
        hero.Input.SetInputLatency(ButtonCode.Left, inputLatency4Kick);
        hero.Input.SetInputLatency(ButtonCode.Jump, inputLatency4Kick);
    }
    public override void Resume(){
        if     (hero.velocity.X > 0) hero.Anim.SetTrigger("jumprf");
        else if(hero.velocity.X < 0) hero.Anim.SetTrigger("jumplf");
        else if(hero.WantsToGoRight)     hero.Anim.SetTrigger("jumpru");
        else                         hero.Anim.SetTrigger("jumplu");
    }

    public override void Update(){
        hero.velocity.Y -= HeroMover.gravity * Time.timeScale;
        if(hero.velocity.Y < 0) hero.States.Push(new StateFall(hero, canJump));
    }

    public override void Exit(){
        hero.Input.SetInputLatency(ButtonCode.Right,0);
        hero.Input.SetInputLatency(ButtonCode.Left,0);
        hero.Input.SetInputLatency(ButtonCode.Jump,0);
    }
}

}