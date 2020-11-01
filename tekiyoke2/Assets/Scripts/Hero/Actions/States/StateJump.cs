using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : HeroState
{
    bool isFromGround;
    float force;
    public StateJump(bool fromGround = true, float force = -1)
    {
        this.isFromGround = fromGround;
        this.force        = force;
    }

    enum Dir{ FR, UR, FL, UL }
    Dir _dir = Dir.UR;
    void SetDir(Dir dir, HeroMover hero)
    {
        if(dir == _dir) return;

        _dir = dir;
        switch(dir)
        {
            case Dir.FR: hero.SetAnimManually("jumpfr"); break;
            case Dir.FL: hero.SetAnimManually("jumpfl"); break;
            case Dir.UR: hero.SetAnimManually("jumpur"); break;
            case Dir.UL: hero.SetAnimManually("jumpul"); break;
        }
    }
    Dir CalcDir(HeroMover hero)
    {
        if(hero.KeyDirection == 1)  return Dir.FR;
        if(hero.KeyDirection == -1) return Dir.FL;
        if(hero.WantsToGoRight)     return Dir.UR;
                                    return Dir.UL;
    }

    IEnumerator kabezuriCoroutine;

    public override void Enter(HeroMover hero)
    {
        hero.velocity.Y = force == -1 ? hero.Parameters.JumpForce : force;

        Start(hero);

        if(isFromGround) hero.ObjsHolderForStates.JumpEffectPool.ActivateOne(hero.WantsToGoRight ? "r" : "l");
        else             hero.ObjsHolderForStates.JumpEffectInAirPool.ActivateOne(hero.WantsToGoRight ? "r" : "l");
        kabezuriCoroutine = hero.SpawnKabezuris(hero.Parameters.MoveInAirParams);
        hero.StartCoroutine(kabezuriCoroutine);

        hero.Jumped(isFromGround, isKick:false);
    }

    public override void Resume(HeroMover hero)
    {
        Start(hero);
        hero.StartCoroutine(kabezuriCoroutine);
    }

    void Start(HeroMover hero)
    {
        hero.SetAnim(hero.KeyDirection == 0 ? "jumpu" : "jumpf");
        _dir = CalcDir(hero);
    }

    public override HeroState HandleInput(HeroMover hero, IAskedInput input)
    {
        if(hero.CanJumpInAir && input.GetButtonDown(ButtonCode.Jump))
        {
            return new StateJump(fromGround: false);
        }

        SetDir(CalcDir(hero), hero);

        return this;
    }
    public override HeroState Update_(HeroMover hero, float deltatime)
    {
        hero.HorizontalMoveInAir(hero.Parameters.MoveInAirParams, deltatime);

        var params_ = hero.Parameters.MoveInAirParams;
        hero.ApplyGravity(params_.Gravity, params_.FallSpeedMax, deltatime);

        if(hero.velocity.Y < 0)
        {
            return new StateFall();
        }

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        hero.StopCoroutine(kabezuriCoroutine);
    }
}
