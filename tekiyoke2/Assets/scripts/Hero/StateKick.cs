using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateKick : IHeroState
{
    static readonly float kickForceY = 40;
    static readonly float moveForce = 0.5f;
    static readonly int frames2BeFree = 20;
    int frames2BeFreeNow = frames2BeFree;
    readonly bool toRight;
    readonly bool canJump;
    HeroMover hero;
    public StateKick(HeroMover hero, bool kick2Right, bool canJump){
        this.hero = hero;
        toRight = kick2Right;
        this.canJump = canJump;
    }
    public void Start(){
        hero.Jumped(false, true);

        if(hero.velocity.y > 0) hero.anim.SetTrigger(toRight ? "jumprf" : "jumplf");
        else                    hero.anim.SetTrigger(toRight ? "fallr"  : "falll");
        hero.EyeToRight = toRight;

        hero.velocity.x = toRight ? HeroMover.moveSpeed : -HeroMover.moveSpeed;
        hero.velocity.y = kickForceY;
    }
    public void Update(){

        if(hero.IsOnGround){
            hero.States.Push(new StateWait(hero));
            return;
        }

        hero.velocity.y -= HeroMover.gravity * Time.timeScale;
        if(hero.velocity.y < 0){
            if(frames2BeFreeNow > 0) hero.anim.SetTrigger(toRight         ? "fallr" : "falll");
            else                     hero.anim.SetTrigger(hero.EyeToRight ? "fallr" : "falll");
        }

        if(frames2BeFreeNow > 0) frames2BeFreeNow --;

        else{
            switch(hero.KeyDirection){

                case 1:
                    if(toRight) hero.velocity.x = HeroMover.moveSpeed;
                    else        hero.velocity.x = System.Math.Min( HeroMover.moveSpeed, hero.velocity.x + moveForce);
                    break;

                case -1:
                    if(toRight) hero.velocity.x = System.Math.Max(-HeroMover.moveSpeed, hero.velocity.x - moveForce);
                    else        hero.velocity.x = -HeroMover.moveSpeed;
                    break;

                case 0:
                    if(toRight) hero.velocity.x = System.Math.Max(0, hero.velocity.x - moveForce / 2);
                    else        hero.velocity.x = System.Math.Min(0, hero.velocity.x + moveForce / 2);
                    break;
            }
        }
    }
    public void Try2StartJet(){
        if(frames2BeFreeNow == 0) hero.States.Push(new StateJet(hero));
    }
    public void Try2EndJet(){ }
    public void Try2Jump(){
        if(frames2BeFreeNow == 0){

            if(hero.CanKickFromWallL)      hero.States.Push(new StateKick(hero, true,  canJump));
            else if(hero.CanKickFromWallR) hero.States.Push(new StateKick(hero, false, canJump));

            else if(canJump) hero.States.Push(new StateJump(hero, false));
        }
    }
    public void Try2StartMove(bool toRight){ }
    public void Try2EndMove(){ }
    public void Exit(){ }
}
