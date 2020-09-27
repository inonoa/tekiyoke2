﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : HeroState
{
    bool canJump;
    float force;
    public StateJump(bool canJump = true, float force = -1)
    {
        this.canJump = canJump;
        this.force   = force;
    }

    enum Dir{ FR, UR, FL, UL }
    Dir _dir = Dir.UR;
    void SetDir(Dir dir, HeroMover hero)
    {
        if(dir == _dir) return;

        _dir = dir;
        switch(dir)
        {
            case Dir.FR: hero.Anim.SetTrigger("jumpfr"); break;
            case Dir.FL: hero.Anim.SetTrigger("jumpfl"); break;
            case Dir.UR: hero.Anim.SetTrigger("jumpur"); break;
            case Dir.UL: hero.Anim.SetTrigger("jumpul"); break;
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

        if(canJump) hero.ObjsHolderForStates.JumpEffectPool.ActivateOne(hero.WantsToGoRight ? "r" : "l");
        else        hero.ObjsHolderForStates.JumpEffectInAirPool.ActivateOne(hero.WantsToGoRight ? "r" : "l");
        kabezuriCoroutine = hero.SpawnKabezuris(hero.Parameters.MoveInAirParams);
        hero.StartCoroutine(kabezuriCoroutine);

        hero.Jumped(isFromGround:canJump, isKick:false);
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
        if(canJump && input.GetButtonDown(ButtonCode.Jump))
        {
            return new StateJump(canJump: false);
        }

        SetDir(CalcDir(hero), hero);

        return this;
    }
    public override HeroState Update_(HeroMover hero, float deltatime)
    {
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
        hero.StopCoroutine(kabezuriCoroutine);
    }
}