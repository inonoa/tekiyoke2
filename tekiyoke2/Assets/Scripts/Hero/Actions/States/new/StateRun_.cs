using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun_ : HeroStateBase
{
    bool right = true;

    float fromNoGround = 0f;

    IEnumerator tsuchihokotiCoroutine;

    public override void Enter(HeroMover hero)
    {
        Init(hero);
    }
    public override void Resume(HeroMover hero)
    {
        Init(hero);
        hero.StartCoroutine(tsuchihokotiCoroutine);
    }

    void Init(HeroMover hero)
    {
        hero.SetAnim("run");
        right = hero.WantsToGoRight;
        hero.SoundGroup.Play("Run");
        tsuchihokotiCoroutine = Tsuchihokori(hero.ObjsHolderForStates.TsuchihokoriPool, hero.Parameters);
        hero.StartCoroutine(tsuchihokotiCoroutine);
    }

    IEnumerator Tsuchihokori(ObjectPool<Tsuchihokori> pool, HeroParameters params_)
    {
        while(true)
        {
            pool.ActivateOne(right ? "r" : "l");
            yield return new WaitForSeconds(params_.TsuchihokoriInterval);
        }
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        if(input.GetButtonDown(ButtonCode.Jump))
        {
            return new StateJump_();
        }
        if(! (input.GetButton(ButtonCode.Right) || input.GetButton(ButtonCode.Left)))
        {
            return new StateWait_();
        }

        if(hero.KeyDirection == 1  && !right)
        {
            right = true;
            hero.SetAnim("run");
        }
        if(hero.KeyDirection == -1 &&  right)
        {
            right = false;
            hero.SetAnim("run");
        }

        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        hero.velocity.Y = 0;

        if(hero.IsOnGround)
        {
            fromNoGround = 0f;
        }
        else
        {
            fromNoGround += deltatime;
            if(fromNoGround >= hero.Parameters.CoyoteTime) return new StateFall_();
        }

        if(hero.KeyDirection == 0)
        {
            return new StateWait_();
        }

        if(     hero.KeyDirection == 1)
        {
            hero.velocity.X = Mathf.Min(
                hero.velocity.X + hero.Parameters.ForceOnGround * deltatime,
                hero.Parameters.GroundSpeedMax);
        }
        else if(hero.KeyDirection == -1)
        {
            hero.velocity.X = Mathf.Max(
                hero.velocity.X - hero.Parameters.ForceOnGround * deltatime,
                -hero.Parameters.GroundSpeedMax);
        }

        hero.ApplySakamichi();
        
        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.StopCoroutine(tsuchihokotiCoroutine);
    }
}