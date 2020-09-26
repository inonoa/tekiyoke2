using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateKick : HeroState
{
    bool canJump = true;
    bool right;

    float fromKick = 0;

    IEnumerator kabezuriCoroutine;

    public StateKick(bool toRight, bool canJump = true)
    {
        this.right = toRight;
        this.canJump = canJump;
    }

    public override void Enter(HeroMover hero)
    {
        HeroVelocity firstSpeed = hero.Parameters.KickParams.KickForce.ToHeroVel();
        if(!right) firstSpeed.X *= -1;
        hero.velocity = firstSpeed;
        hero.CanMove = false;

        hero.SetAnim("jumpf");
        hero.ObjsHolderForStates.JumpEffectPool.ActivateOne(right ? "kr" : "kl");
        kabezuriCoroutine = hero.SpawnKabezuris(hero.Parameters.MoveInAirParams);
        hero.StartCoroutine(kabezuriCoroutine);

        hero.Jumped(isFromGround:false, isKick:true);
    }
    public override void Resume(HeroMover hero)
    {
        hero.SetAnim("jumpf");
        hero.StartCoroutine(kabezuriCoroutine);
    }

    public override HeroState HandleInput(HeroMover hero, IAskedInput input)
    {
        if(hero.IsReady2Kick2Left(input))  return new StateKick(toRight: false, canJump);
        if(hero.IsReady2Kick2Right(input)) return new StateKick(toRight: true,  canJump);

        if(input.GetButtonDown(ButtonCode.Jump))
        {
            if(canJump) return new StateJump(canJump: false);
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

        hero.HorizontalMoveInAir(hero.Parameters.MoveInAirParams, deltatime);

        hero.ApplyGravity(hero.Parameters.MoveInAirParams, deltatime);

        if(hero.velocity.Y < 0)
        {
            return new StateFall(canJump);
        }

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.CanMove = true; //これええんかな
        hero.StopCoroutine(kabezuriCoroutine);
    }
}
