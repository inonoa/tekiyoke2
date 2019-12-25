using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : IHeroState
{
    static readonly float sakamichiSpeedRate = 1.5f;
    HeroMover hero;
    public StateRun(HeroMover hero){
        this.hero = hero;
    }
    public void Try2StartJet(){
        //今のところパス
    }
    public void Try2EndJet(){ }
    public void Try2Jump(){
        hero.States.Push(new StateJump(hero));
        if(hero.EyeToRight) hero.velocity.x = HeroMover.moveSpeed;
        else hero.velocity.x = -HeroMover.moveSpeed;
    }
    public void Try2StartMove(bool toRight){
        if(toRight){
            hero.velocity.x =  HeroMover.moveSpeed;
            hero.anim.SetTrigger("runr");
        }
        else{
            hero.velocity.x = -HeroMover.moveSpeed;
            hero.anim.SetTrigger("runl");
        }
    }
    public void Try2EndMove(){
        hero.States.Push(new StateWait(hero));
    }
    public void Start(){
        hero.velocity.x = hero.EyeToRight ? HeroMover.moveSpeed : -HeroMover.moveSpeed;
        hero.velocity.y = 0;
        hero.anim.SetTrigger(hero.EyeToRight ? "runr" : "runl");
    }
    public void Update(){
        if(!hero.IsOnGround) hero.States.Push(new StateFall(hero));

        //坂を右向きに上っているときは数値上若干加速し、下っているときは下に落とすことで接地し続けさせる
        if(hero.IsOnSakamichiR){
            if(hero.EyeToRight){
                hero.velocity.x =  HeroMover.moveSpeed * sakamichiSpeedRate;
                hero.velocity.y = 0;
            }
            else{
                hero.velocity.x = -HeroMover.moveSpeed;
                hero.velocity.y = -20;
            }

        //坂を左向きに上っているときは数値上若干加速し、下っているときは下に落とすことで接地し続けさせる
        }else if(hero.IsOnSakamichiL){
            if(!hero.EyeToRight){
                hero.velocity.x = -HeroMover.moveSpeed * sakamichiSpeedRate;
                hero.velocity.y = 0;
            }else{
                hero.velocity.x = HeroMover.moveSpeed;
                hero.velocity.y = -20;
            }

        //そうでなければまあ良しなに
        }else{
            if(hero.EyeToRight) hero.velocity.x = HeroMover.moveSpeed;
            else hero.velocity.x = -HeroMover.moveSpeed;
            hero.velocity.y = 0;
        }
    }
}
