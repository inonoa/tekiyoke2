using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateKick : HeroState
{
    static readonly float kickForceY = 30;
    static readonly float moveForce = 0.38f;
    static readonly int frames2BeFree = 20;
    int frames2BeFreeNow = frames2BeFree;
    readonly bool toRight;
    readonly bool canJump;

    static readonly float kabezuriInterval = 0.1f;
    Coroutine kabezuriCoroutine;
    HeroMover hero;
    public StateKick(HeroMover hero, bool kick2Right, bool canJump){
        this.hero = hero;
        toRight = kick2Right;
        this.canJump = canJump;
    }
    public override void Start(){
        hero.Jumped(false, true);

        hero.anim.SetTrigger(toRight ? "jumprf" : "jumplf");
        hero.EyeToRight = toRight;

        hero.objsHolderForStates.JumpEffectPool.ActivateOne(toRight ? "kr" : "kl");
        kabezuriCoroutine = hero.StartCoroutine(SpawnKabezuris());

        hero.velocity.x = toRight ? HeroMover.moveSpeed : -HeroMover.moveSpeed;
        hero.velocity.y = kickForceY;
    }


    IEnumerator SpawnKabezuris(){
        Try2SpawnKabezuri();

        while(true){
            yield return new WaitForSeconds(kabezuriInterval);

            Try2SpawnKabezuri();
        }
    }

    void Try2SpawnKabezuri(){
        if(hero.velocity.y > 0) return;

        bool dir_is_R;

        if(hero.CanKickFromWallR && hero.CanKickFromWallL) dir_is_R = hero.EyeToRight;
        else if(hero.CanKickFromWallR)                     dir_is_R = true;
        else if(hero.CanKickFromWallL)                     dir_is_R = false;
        else return;

        hero.objsHolderForStates.KabezuriPool.ActivateOne(dir_is_R ? "r" : "l");
    }

    public override void Resume(){
        if(hero.velocity.y > 0) hero.anim.SetTrigger(toRight ? "jumprf" : "jumplf");
        else                    hero.anim.SetTrigger(toRight ? "fallr"  : "falll");
    }

    public override void Update(){

        if(hero.IsOnGround && hero.velocity.y <= 0){
            hero.States.Push(new StateWait(hero));
            return;
        }

        hero.velocity.y -= HeroMover.gravity * Time.timeScale;
        if(hero.velocity.y < 0){
            if(frames2BeFreeNow > 0) hero.anim.SetTrigger(toRight         ? "fallr" : "falll");
            else                     hero.anim.SetTrigger(hero.EyeToRight ? "fallr" : "falll");
            //これ単にFallに遷移するほうがいいんじゃないの……？
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
    public override void Try2StartJet(){
        if(frames2BeFreeNow == 0) hero.States.Push(new StateJet(hero));
    }
    public override void Try2EndJet(){ }
    public override void Try2Jump(){
        if(frames2BeFreeNow == 0){

            if(hero.CanKickFromWallL)      hero.States.Push(new StateKick(hero, true,  canJump));
            else if(hero.CanKickFromWallR) hero.States.Push(new StateKick(hero, false, canJump));

            else if(canJump) hero.States.Push(new StateJump(hero, false));
        }
    }
    public override void Try2StartMove(bool toRight){ }
    public override void Try2EndMove(){ }
    public override void Exit(){
        hero.StopCoroutine(kabezuriCoroutine);
    }
}
