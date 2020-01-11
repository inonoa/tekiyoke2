using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJet : IHeroState
{
    static readonly float timeScaleBeforeJet = 0.2f;

    HeroMover hero;

    enum State { Ready, Jetting }
    State state = State.Ready;
    bool jet2Right = true;

    int tameFrames = 0;
    int jetFrames = 0;
    int jetFramesMax;

    float[] jetVelocities;

    public StateJet(HeroMover hero){
        this.hero = hero;
    }
    public void Try2StartJet(){ }
    public void Try2EndJet(){
        if(state==State.Ready){
            hero.CanBeDamaged = false;
            state = State.Jetting;
            hero.anim.SetTrigger(jet2Right ? "runr" : "runl");
            Tokitome.SetTime(1);
            hero.spriteRenderer.color = new Color(1,1,1,0.3f);

            //Jetする距離を計算
            int fullDist = (int)(MyMath.FloorAndCeil(10,tameFrames,30) * 25);
            jetFramesMax = (fullDist*3) /100;
            jetVelocities = new float[jetFramesMax];
            for(int i=0;i<jetFramesMax;i++)
                jetVelocities[i] = fullDist * ( EasingFunc((i+1)/(float)jetFramesMax) - EasingFunc(i/(float)jetFramesMax) );

            //ちょっと待って…
            // phantom.SetActive(false);
            hero.cmrCntr.Dash(jetFramesMax); //今は何も起こってなさそう
        }
    }
    public void Try2Jump(){ }
    public void Try2StartMove(bool toRight){
        if(state==State.Ready) jet2Right = toRight;
    }
    public void Try2EndMove(){ }
    public void Start(){
        Tokitome.SetTime(timeScaleBeforeJet);

        switch(hero.KeyDirection){
            case 1:
                jet2Right = true;
                break;
            case -1:
                jet2Right = false;
                break;
            case 0:
                jet2Right = hero.EyeToRight;
                break;
        }

        hero.cmrCntr.StartZoomForDash();
    }
    public void Update(){
        switch(state){

            case State.Ready:
                //ためすぎるとエンスト？してダメージ受けるとかしたいね
                tameFrames ++;
                if(!hero.IsOnGround) hero.velocity.y -= HeroMover.gravity;
                break;

            case State.Jetting:
                hero.velocity = (jet2Right ? jetVelocities[jetFrames] : -jetVelocities[jetFrames] , 0);

                jetFrames ++;
                if(jetFrames == jetFramesMax) hero.States.Push(new StateWait(hero));
                break;
        }
    }

    static float EasingFunc(float t){
        if(t>1) Debug.Log("t>1になっています");
        if(t<0) Debug.Log("t<0になっています");
        
        float zero2one = MyMath.FloorAndCeil(0,t,1);

        if(zero2one<0.2f)
            return 4 * zero2one;
        else
            return 0.8f + 0.25f * (zero2one - 0.2f);
    }

    public void Exit(){
        hero.CanBeDamaged = true;
        hero.spriteRenderer.color = new Color(1,1,1,1);
        hero.cmrCntr.EndDash();
    }
}
