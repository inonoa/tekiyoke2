using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : IHeroState
{
    //こういうのをScriptableObjectにしたらいじりやすくなるな確かに
    static readonly float sakamichiSpeedRate = 1.5f;
    static readonly float tsuchihokoriInterval = 0.25f; //アニメと一致させたいな～～～
    HeroMover hero;

    Coroutine tsuchihokoriCoroutine;
    public StateRun(HeroMover hero){
        this.hero = hero;
    }
    public void Try2StartJet(){
        hero.States.Push(new StateJet(hero));
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
        tsuchihokoriCoroutine = hero.StartCoroutine(Tsuchihokori());
    }

    IEnumerator Tsuchihokori(){
        Debug.Log("Run!");

        while(true){
            yield return new WaitForSeconds(tsuchihokoriInterval);
            Debug.Log("tsuchihokori");
        }
    }

    public void Update(){
        if(!hero.IsOnGround) hero.States.Push(new StateFall(hero));

        //坂を右向きに上っているときは数値上若干加速し、下っているときは下に落とすことで接地し続けさせる
        if(hero.IsOnSakamichiR){
            if(hero.KeyDirection==1){
                hero.velocity.x =  HeroMover.moveSpeed * sakamichiSpeedRate;
                hero.velocity.y = 0;
            }
            else{
                hero.velocity.x = -HeroMover.moveSpeed;
                hero.velocity.y = -20;
            }

        //坂を左向きに上っているときは数値上若干加速し、下っているときは下に落とすことで接地し続けさせる
        }else if(hero.IsOnSakamichiL){
            if(!(hero.KeyDirection==1)){
                hero.velocity.x = -HeroMover.moveSpeed * sakamichiSpeedRate;
                hero.velocity.y = 0;
            }else{
                hero.velocity.x = HeroMover.moveSpeed;
                hero.velocity.y = -20;
            }

        //そうでなければまあ良しなに
        }else{
            if(hero.KeyDirection==1) hero.velocity.x = HeroMover.moveSpeed;
            else hero.velocity.x = -HeroMover.moveSpeed;
            hero.velocity.y = 0;
        }
    }

    public void Exit(){
        hero.StopCoroutine(tsuchihokoriCoroutine);
    }
}
