using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFall : HeroState
{
    bool right = true;
    bool canJump = true;

    IEnumerator kabezuriCoroutine;

    public StateFall(bool canJump = true)
    {
        this.canJump = canJump;
    }

    public override void Enter(HeroMover hero)
    {
        Start(hero);
        kabezuriCoroutine = hero.SpawnKabezuris(hero.Parameters.MoveInAirParams);
        hero.StartCoroutine(kabezuriCoroutine);
    }
    public override void Resume(HeroMover hero)
    {
        Start(hero);
        hero.StartCoroutine(kabezuriCoroutine);
    }

    void Start(HeroMover hero)
    {
        right = hero.WantsToGoRight;
        hero.SetAnim("fall");
    }

    public override HeroState HandleInput(HeroMover hero, IAskedInput input)
    {
        if(hero.IsReady2Kick2Left(input))  return new StateKick(toRight: false, canJump);
        if(hero.IsReady2Kick2Right(input)) return new StateKick(toRight: true,  canJump);

        if(input.GetButtonDown(ButtonCode.Jump))
        {
            if(canJump) return new StateJump(canJump: false);
        }

        if(     hero.KeyDirection == 1  && !right)
        {
            right = true;
            hero.SetAnim("fall");
        }
        else if(hero.KeyDirection == -1 &&  right)
        {
            right = false;
            hero.SetAnim("fall");
        }

        return this;
    }

    public override HeroState Update_(HeroMover hero, float deltatime)
    {
        hero.HorizontalMoveInAir(hero.Parameters.MoveInAirParams, deltatime);

        var params_ = hero.Parameters.MoveInAirParams;
        hero.ApplyGravity(params_.Gravity, params_.FallSpeedMax, deltatime);

        if(hero.IsOnGround)
        {
            hero.SoundGroup.Play("Land");
            if(hero.KeyDirection == 0) return new StateWait();
            else                       return new StateRun();
        }

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.StopCoroutine(kabezuriCoroutine);
    }
}
