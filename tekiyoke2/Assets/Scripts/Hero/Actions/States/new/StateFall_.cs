using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFall_ : HeroStateBase
{
    bool right = true;
    bool canJump = true;

    IEnumerator kabezuriCoroutine;

    public StateFall_(bool canJump = true)
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

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        if(hero.IsReady2Kick2Left(input))  return new StateKick_(toRight: false, canJump);
        if(hero.IsReady2Kick2Right(input)) return new StateKick_(toRight: true,  canJump);

        if(input.GetButtonDown(ButtonCode.Jump))
        {
            if(canJump) return new StateJump_(canJump: false);
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

    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        hero.HorizontalMoveInAir(hero.Parameters.MoveInAirParams, deltatime);

        hero.ApplyGravity(hero.Parameters.MoveInAirParams, deltatime);

        if(hero.IsOnGround)
        {
            hero.SoundGroup.Play("Land");
            if(hero.KeyDirection == 0) return new StateWait_();
            else                       return new StateRun_();
        }

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.StopCoroutine(kabezuriCoroutine);
    }
}
