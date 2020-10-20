using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldStates{

public class StateBend : HeroState
{
    HeroMover hero;
    static readonly (float X, float Y) bendForce = (15, 10);
    static readonly int bendFrames = 100;
    int bendFramesNow = bendFrames;
    static readonly int frames_CantUpdate = 3;
    int framesAfterBent = 0;
    public StateBend(HeroMover hero){ // 引き数にtoRight入れる？
        this.hero = hero;
    }
    public override void Start(){
        if(hero.velocity.X != 0)
            hero.velocity.X = (hero.velocity.X > 0) ? -bendForce.X : bendForce.X;
        else
            hero.velocity.X =  hero.WantsToGoRight      ? -bendForce.X : bendForce.X;
        
        hero.velocity.Y = bendForce.Y;

        hero.Anim.SetTrigger( (hero.velocity.X > 0) ? "jumprf" : "jumplf" );

        //hero.CanBeDamaged = false;

        CameraController.CurrentCamera.Freeze(num_frames: 20);
    }

    public override void Resume(){
        hero.Anim.SetTrigger( (hero.velocity.X > 0) ? "jumprf" : "jumplf" );
    }

    public override void Update(){

        if(framesAfterBent == frames_CantUpdate){
            if(hero.IsOnGround) hero.States.Push(new StateWait(hero));

            bendFramesNow --;
            if(bendFramesNow == 0) hero.States.Push(new StateWait(hero));

            hero.velocity.Y -= HeroMover.gravity * Time.timeScale;

        }else{
            framesAfterBent ++;
        }
    }
    public override void Try2StartJet(){ }
    public override void Try2EndJet(){ }
    public override void Try2Jump(){ }
    public override void Try2StartMove(bool toRight){ }
    public override void Try2EndMove(){ }
    public override void Exit(){

        if(hero.HpCntr.HP > 0){
            hero.CanMove = true;
            //hero.CanBeDamaged = true;
        }
    }
}

}