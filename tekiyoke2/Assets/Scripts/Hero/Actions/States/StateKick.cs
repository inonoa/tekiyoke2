using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateKick : HeroState, IAskCanJump
{
    static readonly float kickForceY = 30;
    static readonly float moveForce = 0.38f;
    static readonly int frames2BeFree = 20;
    int frames2BeFreeNow = frames2BeFree;
    readonly bool toRight;
    readonly bool canJump;
    public bool CanJump => canJump;

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

        hero.Anim.SetTrigger(toRight ? "jumprf" : "jumplf");
        //hero.EyeToRight = toRight;
        hero.SoundGroup.Play("Jump");

        hero.ObjsHolderForStates.JumpEffectPool.ActivateOne(toRight ? "kr" : "kl");
        kabezuriCoroutine = hero.StartCoroutine(SpawnKabezuris());

        hero.velocity.X = toRight ? HeroMover.moveSpeed : -HeroMover.moveSpeed;
        hero.velocity.Y = kickForceY;
    }


    IEnumerator SpawnKabezuris(){
        Try2SpawnKabezuri();

        while(true){
            yield return new WaitForSeconds(kabezuriInterval);

            Try2SpawnKabezuri();
        }
    }

    void Try2SpawnKabezuri(){
        if(hero.velocity.Y > 0) return;

        bool dir_is_R;

        if(hero.CanKickFromWallR && hero.CanKickFromWallL) dir_is_R = hero.WantsToGoRight;
        else if(hero.CanKickFromWallR)                     dir_is_R = true;
        else if(hero.CanKickFromWallL)                     dir_is_R = false;
        else return;

        hero.ObjsHolderForStates.KabezuriPool.ActivateOne(dir_is_R ? "r" : "l");
    }

    public override void Resume(){
        if(hero.velocity.Y > 0) hero.Anim.SetTrigger(toRight ? "jumprf" : "jumplf");
        else                    hero.Anim.SetTrigger(toRight ? "fallr"  : "falll");
    }

    public override void Update(){

        if(hero.IsOnGround && hero.velocity.Y <= 0){
            hero.States.Push(new StateWait(hero));
            return;
        }

        hero.velocity.Y -= HeroMover.gravity * Time.timeScale;
        if(hero.velocity.Y < 0){
            if(frames2BeFreeNow > 0) hero.Anim.SetTrigger(toRight         ? "fallr" : "falll");
            else                     hero.Anim.SetTrigger(hero.WantsToGoRight ? "fallr" : "falll");
            //これ単にFallに遷移するほうがいいんじゃないの……？
        }

        if(frames2BeFreeNow > 0) frames2BeFreeNow --;

        else{
            switch(hero.KeyDirection){

                case 1:
                    if(toRight) hero.velocity.X = HeroMover.moveSpeed;
                    else        hero.velocity.X = System.Math.Min( HeroMover.moveSpeed, hero.velocity.X + moveForce);
                    break;

                case -1:
                    if(toRight) hero.velocity.X = System.Math.Max(-HeroMover.moveSpeed, hero.velocity.X - moveForce);
                    else        hero.velocity.X = -HeroMover.moveSpeed;
                    break;

                case 0:
                    if(toRight) hero.velocity.X = System.Math.Max(0, hero.velocity.X - moveForce / 2);
                    else        hero.velocity.X = System.Math.Min(0, hero.velocity.X + moveForce / 2);
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
