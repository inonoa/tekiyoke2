using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateKick : HeroState
{
    readonly bool right;

    float fromKick = 0;

    IEnumerator kabezuriCoroutine;

    public StateKick(bool toRight)
    {
        this.right = toRight;
    }

    public override void Enter(HeroMover hero)
    {
        HeroVelocity firstSpeed = hero.Parameters.KickParams.KickForce.ToHeroVel();
        if(!right) firstSpeed.X *= -1;
        hero.velocity = firstSpeed;
        hero.CanMove = false;

        hero.SetAnimManually(right ? "jumpfr" : "jumpfl");
        if(hero.KeyDirection == 0) hero.ForceChangeWantsToGoRight(right); //しっくりこない
        hero.SoundGroup.Play("Jump");
        hero.ObjsHolderForStates.JumpEffectPool.ActivateOne(right ? "kr" : "kl");
        kabezuriCoroutine = hero.SpawnKabezuris(hero.Parameters.MoveInAirParamsAfterKick);
        hero.StartCoroutine(kabezuriCoroutine);

        hero.Jumped(isFromGround:false, isKick:true);
    }
    public override void Resume(HeroMover hero)
    {
        hero.SetAnimManually(right ? "jumpfr" : "jumpfl");
        hero.StartCoroutine(kabezuriCoroutine);
    }

    public override HeroState HandleInput(HeroMover hero, IAskedInput input)
    {
        if(hero.IsReady2Kick2Left(input))  return new StateKick(toRight: false);
        if(hero.IsReady2Kick2Right(input)) return new StateKick(toRight: true);

        if(input.GetButtonDown(ButtonCode.Jump))
        {
            if(hero.CanJumpInAir) return new StateJump(fromGround: false);
        }

        return this;
    }
    public override HeroState Update_(HeroMover hero, float deltatime)
    {
        if(fromKick < hero.Parameters.KickParams.FromKickToInputEnabled)
        {
            fromKick += deltatime;
            if(fromKick >= hero.Parameters.KickParams.FromKickToInputEnabled)
            {
                hero.CanMove = true;
            }
        }

        hero.HorizontalMoveInAir(hero.Parameters.MoveInAirParamsAfterKick, deltatime);

        var params_ = hero.Parameters.MoveInAirParamsAfterKick;
        hero.ApplyGravity(params_.Gravity, params_.FallSpeedMax, deltatime);

        if(hero.velocity.Y < 0)
        {
            return new StateFall();
        }

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.CanMove = true; //これええんかな
        hero.StopCoroutine(kabezuriCoroutine);
    }
}
