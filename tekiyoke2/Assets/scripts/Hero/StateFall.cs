using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFall : HeroState, IAskCanJump
{
    static readonly int inputLatency4Kick = 3;
    readonly bool canJump;
    public bool CanJump => canJump;
    static readonly int coyoteTime = 10;

    static readonly float kabezuriInterval = 0.1f;
    Coroutine kabezuriCoroutine;

    HeroMover hero;

    public StateFall(HeroMover hero, bool canJump = true){
        this.hero = hero;
        this.canJump = canJump;
    }
    public override void Try2StartJet(){
        hero.States.Push(new StateJet(hero));
    }
    public override void Try2EndJet(){ }
    public override void Try2Jump(){
        IAskedInput input = InputManager.Instance;

        if(hero.CanKickFromWallL && input.GetButton(ButtonCode.Left) && input.GetButtonDown(ButtonCode.Jump)){
            hero.States.Push(new StateKick(hero, true,  canJump));
            
        }else if(hero.CanKickFromWallR && input.GetButton(ButtonCode.Right) && input.GetButtonDown(ButtonCode.Jump)){
            hero.States.Push(new StateKick(hero, false, canJump));

        }else if(canJump){
            if(hero.FramesSinceTakeOff < coyoteTime) hero.States.Push(new StateJump(hero, true));
            else hero.States.Push(new StateJump(hero, false));
        }
    }
    public override void Try2StartMove(bool toRight){
        IAskedInput input = InputManager.Instance;

        if(toRight){
            if(hero.CanKickFromWallR && input.GetButton(ButtonCode.Right) && input.GetButtonDown(ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, false, canJump));

            hero.velocity.X =  HeroMover.moveSpeed;
            hero.Anim.SetTrigger("fallr");

        }else{
            if(hero.CanKickFromWallL && input.GetButton(ButtonCode.Left) && input.GetButtonDown(ButtonCode.Jump))
                hero.States.Push(new StateKick(hero, true,  canJump));
                
            hero.velocity.X = -HeroMover.moveSpeed;
            hero.Anim.SetTrigger("falll");
        }
    }
    public override void Try2EndMove(){
        hero.velocity.X = 0;
    }
    public override void Start(){
        hero.Anim.SetTrigger(hero.EyeToRight ? "fallr" : "falll");
        kabezuriCoroutine = hero.StartCoroutine(SpawnKabezuris());
        switch(hero.KeyDirection){
            case 1 : hero.velocity.X = HeroMover.moveSpeed;  break;
            case 0 : hero.velocity.X = 0;                    break;
            case -1: hero.velocity.X = -HeroMover.moveSpeed; break;
        }
        InputManager.Instance.SetInputLatency(ButtonCode.Right,inputLatency4Kick);
        InputManager.Instance.SetInputLatency(ButtonCode.Left, inputLatency4Kick);
        InputManager.Instance.SetInputLatency(ButtonCode.Jump, inputLatency4Kick);
    }

    public override void Resume(){
        hero.Anim.SetTrigger(hero.EyeToRight ? "fallr" : "falll");
    }

    IEnumerator SpawnKabezuris(){
        Try2SpawnKabezuri();

        while(true){
            yield return new WaitForSeconds(kabezuriInterval);

            Try2SpawnKabezuri();
        }
    }

    void Try2SpawnKabezuri(){
        bool dir_is_R;

        if(hero.CanKickFromWallR && hero.CanKickFromWallL) dir_is_R = hero.EyeToRight;
        else if(hero.CanKickFromWallR)                     dir_is_R = true;
        else if(hero.CanKickFromWallL)                     dir_is_R = false;
        else return;

        hero.ObjsHolderForStates.KabezuriPool.ActivateOne(dir_is_R ? "r" : "l");
    }

    public override void Update(){
        hero.velocity.Y -= HeroMover.gravity * Time.timeScale;
        if(hero.IsOnGround){
            hero.SoundGroup.Play("Land");
            if(hero.KeyDirection==0) hero.States.Push(new StateWait(hero));
            else hero.States.Push(new StateRun(hero));
        }
    }

    public override void Exit(){
        hero.StopCoroutine(kabezuriCoroutine);

        InputManager.Instance.SetInputLatency(ButtonCode.Right,0);
        InputManager.Instance.SetInputLatency(ButtonCode.Left,0);
        InputManager.Instance.SetInputLatency(ButtonCode.Jump,0);
    }
}
