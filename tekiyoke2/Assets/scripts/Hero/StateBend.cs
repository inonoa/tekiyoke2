using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBend : IHeroState
{
    HeroMover hero;
    static readonly (float x, float y) bendForce = (15, 10);
    static readonly int bendFrames = 100;
    int bendFramesNow = bendFrames;
    static readonly int frames_CantUpdate = 3;
    int framesAfterBent = 0;
    public StateBend(HeroMover hero){ // 引き数にtoRight入れる？
        this.hero = hero;
    }
    public void Start(){
        if(hero.velocity.x != 0)
            hero.velocity.x = (hero.velocity.x > 0) ? -bendForce.x : bendForce.x;
        else
            hero.velocity.x =  hero.EyeToRight      ? -bendForce.x : bendForce.x;
        
        hero.velocity.y = bendForce.y;

        hero.anim.SetTrigger( (hero.velocity.x > 0) ? "jumprf" : "jumplf" );

        hero.CanBeDamaged = false;

        CameraController.CurrentCamera.Freeze(num_frames: 20);
        Debug.Log("ヴッ");
    }
    public void Update(){

        if(framesAfterBent == frames_CantUpdate){
            if(hero.IsOnGround) hero.States.Push(new StateWait(hero));

            bendFramesNow --;
            if(bendFramesNow == 0) hero.States.Push(new StateWait(hero));

            hero.velocity.y -= HeroMover.gravity * Time.timeScale;

        }else{
            framesAfterBent ++;
        }
    }
    public void Try2StartJet(){ }
    public void Try2EndJet(){ }
    public void Try2Jump(){ }
    public void Try2StartMove(bool toRight){ }
    public void Try2EndMove(){ }
    public void Exit(){ 
        hero.CanMove = true;
        hero.CanBeDamaged = true;
    }
}
