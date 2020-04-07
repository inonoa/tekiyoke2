using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : IHeroState
{
    static readonly int inputLatency4Kick = 3;
    float jumpForce;
    HeroMover hero;
    readonly bool canJump;
    public StateJump(HeroMover hero, bool canJump = true, float jumpForce = 30){
        this.hero = hero;
        this.canJump = canJump;
        this.jumpForce = jumpForce;
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

        else if(canJump) hero.States.Push(new StateJump(hero, false));
    }
    public void Try2StartMove(bool toRight){

        if(toRight){
            if(hero.CanKickFromWallL && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Right,ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, true,  canJump));

            hero.velocity.x =  HeroMover.moveSpeed;
            hero.anim.SetTrigger("jumprf");

        }else{
            if(hero.CanKickFromWallR && InputManager.Instance.ButtonsDownSimultaneously(ButtonCode.Left,ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, false, canJump));

            hero.velocity.x = -HeroMover.moveSpeed;
            hero.anim.SetTrigger("jumplf");
        }
    }
    public void Try2EndMove(){
        hero.velocity.x = 0;
        hero.anim.SetTrigger(hero.EyeToRight ? "jumpru" : "jumplu");
    }
    public void Start(){
        hero.velocity.y = jumpForce;
        hero.Jumped(canJump, false);

        if     (hero.velocity.x > 0) hero.anim.SetTrigger("jumprf");
        else if(hero.velocity.x < 0) hero.anim.SetTrigger("jumplf");
        else if(hero.EyeToRight)     hero.anim.SetTrigger("jumpru");
        else                         hero.anim.SetTrigger("jumplu");

        if(canJump) hero.objsHolderForStates.JumpEffectPool.ActivateOne(hero.EyeToRight ? "r" : "l");
        
        InputManager.Instance.SetInputLatency(ButtonCode.Right,inputLatency4Kick);
        InputManager.Instance.SetInputLatency(ButtonCode.Left, inputLatency4Kick);
        InputManager.Instance.SetInputLatency(ButtonCode.Jump, inputLatency4Kick);
    }
    public void Update(){
        hero.velocity.y -= HeroMover.gravity * Time.timeScale;
        if(hero.velocity.y < 0) hero.States.Push(new StateFall(hero, canJump));
    }

    public void Exit(){
        InputManager.Instance.SetInputLatency(ButtonCode.Right,0);
        InputManager.Instance.SetInputLatency(ButtonCode.Left,0);
        InputManager.Instance.SetInputLatency(ButtonCode.Jump,0);
    }
}
