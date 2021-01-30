using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : HeroState
{
    bool right = true;

    float fromNoGround = 0f;

    IEnumerator tsuchihokotiCoroutine;

    public override void Enter(HeroMover hero)
    {
        Init(hero);
        tsuchihokotiCoroutine = Tsuchihokori(hero.ObjsHolderForStates.TsuchihokoriPool, hero.Parameters, hero);
        hero.StartCoroutine(tsuchihokotiCoroutine);
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
    }

    IEnumerator Tsuchihokori(ObjectPool<Tsuchihokori> pool, HeroParameters params_, HeroMover hero)
    {
        while(true)
        {
            pool.ActivateOne(right ? "r" : "l");

            float time = 0;
            yield return null;
            while((time += hero.TimeManager.DeltaTimeAroundHero) < params_.RunParams.TsuchihokoriInterval)
            {
                yield return null;
            }
        }
    }

    public override HeroState HandleInput(HeroMover hero, IInput input)
    {
        if(input.GetButtonDown(ButtonCode.Jump))
        {
            return new StateJump();
        }
        if(! (input.GetButton(ButtonCode.Right) || input.GetButton(ButtonCode.Left)))
        {
            return new StateWait();
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
    public override HeroState Update_(HeroMover hero, float deltatime)
    {
        hero.velocity.Y = 0;

        if(hero.IsOnGround)
        {
            fromNoGround = 0f;
        }
        else
        {
            fromNoGround += deltatime;
            if(fromNoGround >= hero.Parameters.RunParams.CoyoteTime) return new StateFall();
        }

        if(hero.KeyDirection == 0)
        {
            return new StateWait();
        }

        if(     hero.KeyDirection == 1)
        {
            hero.velocity.X = Mathf.Min(
                hero.velocity.X + hero.Parameters.RunParams.ForceOnGround * deltatime,
                hero.Parameters.RunParams.GroundSpeedMax);
        }
        else if(hero.KeyDirection == -1)
        {
            hero.velocity.X = Mathf.Max(
                hero.velocity.X - hero.Parameters.RunParams.ForceOnGround * deltatime,
                -hero.Parameters.RunParams.GroundSpeedMax);
        }

        hero.ApplySakamichi();
        
        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.SoundGroup.Stop("Run");
        hero.StopCoroutine(tsuchihokotiCoroutine);
    }
}