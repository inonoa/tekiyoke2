using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : HeroState
{
    //こういうのをScriptableObjectにしたらいじりやすくなるな確かに
    static readonly float sakamichiSpeedRate = 1.5f;
    static readonly float tsuchihokoriInterval = 0.1f; //アニメと一致させたいな～～～
    HeroMover hero;

    Coroutine tsuchihokoriCoroutine;
    public StateRun(HeroMover hero){
        this.hero = hero;
    }
    public override void Try2StartJet(){
        hero.States.Push(new StateJet(hero));
    }
    public override void Try2EndJet(){ }
    public override void Try2Jump(){
        hero.States.Push(new StateJump(hero));
        if(hero.WantsToGoRight) hero.velocity.X = HeroMover.moveSpeed;
        else hero.velocity.X = -HeroMover.moveSpeed;
    }
    public override void Try2StartMove(bool toRight){
        if(toRight){
            hero.velocity.X =  HeroMover.moveSpeed;
            hero.Anim.SetTrigger("runr");
        }
        else{
            hero.velocity.X = -HeroMover.moveSpeed;
            hero.Anim.SetTrigger("runl");
        }
    }
    public override void Try2EndMove(){
        hero.States.Push(new StateWait(hero));
    }
    public override void Start(){
        hero.velocity.X = hero.WantsToGoRight ? HeroMover.moveSpeed : -HeroMover.moveSpeed;
        hero.velocity.Y = 0;
        hero.Anim.SetTrigger(hero.WantsToGoRight ? "runr" : "runl");
        tsuchihokoriCoroutine = hero.StartCoroutine(Tsuchihokoris());
        hero.SoundGroup.Play("Run");
    }

    IEnumerator Tsuchihokoris(){
        Tsuchihokori();

        while(true){
            yield return new WaitForSeconds(tsuchihokoriInterval);

            Tsuchihokori();
        }
    }

    void Tsuchihokori(){
        hero.ObjsHolderForStates.TsuchihokoriPool.ActivateOne(hero.WantsToGoRight ? "r" : "l");
    }

    public override void Resume(){
        hero.Anim.SetTrigger(hero.WantsToGoRight ? "runr" : "runl");
    }

    public override void Update(){
        if(!hero.IsOnGround) hero.States.Push(new StateFall(hero));

        //坂を右向きに上っているときは数値上若干加速し、下っているときは下に落とすことで接地し続けさせる
        if(hero.IsOnSakamichiR){
            if(hero.KeyDirection==1){
                hero.velocity.X =  HeroMover.moveSpeed * sakamichiSpeedRate;
                hero.velocity.Y = 0;
            }
            else{
                hero.velocity.X = -HeroMover.moveSpeed;
                hero.velocity.Y = -20;
            }

        //坂を左向きに上っているときは数値上若干加速し、下っているときは下に落とすことで接地し続けさせる
        }else if(hero.IsOnSakamichiL){
            if(!(hero.KeyDirection==1)){
                hero.velocity.X = -HeroMover.moveSpeed * sakamichiSpeedRate;
                hero.velocity.Y = 0;
            }else{
                hero.velocity.X = HeroMover.moveSpeed;
                hero.velocity.Y = -20;
            }

        //そうでなければまあ良しなに
        }else{
            if(hero.KeyDirection==1) hero.velocity.X = HeroMover.moveSpeed;
            else hero.velocity.X = -HeroMover.moveSpeed;
            hero.velocity.Y = 0;
        }
    }

    public override void Exit(){
        hero.StopCoroutine(tsuchihokoriCoroutine);
        hero.SoundGroup.Stop("Run");
    }
}
