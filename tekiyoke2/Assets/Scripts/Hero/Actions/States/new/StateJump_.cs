using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump_ : HeroStateBase
{
    float jumpForce = 30f;
    float gravity = 80f;
    float horizontalMoveForce = 150f;
    float horizontalMoveSpeed = 15f;

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

    public override void Enter(HeroMover hero)
    {
        hero.velocity.Y = jumpForce;

        Start(hero);
    }
    public override void Resume(HeroMover hero)
    {
        Start(hero);
    }

    void Start(HeroMover hero)
    {
        hero.SetAnim(hero.KeyDirection == 0 ? "jumpu" : "jumpf");
        _dir = CalcDir(hero);
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
        SetDir(CalcDir(hero), hero);

        return this;
    }
    public override HeroStateBase Update_(HeroMover hero, float deltatime)
    {
        hero.velocity.X = Mathf.Clamp(
            hero.velocity.X + hero.KeyDirection * horizontalMoveForce * deltatime,
            -horizontalMoveSpeed,
             horizontalMoveSpeed
        );
        hero.velocity.Y -= gravity * deltatime;

        if(hero.IsOnGround && hero.velocity.Y < 0)
        {
            //共通化したい
            hero.SoundGroup.Play("Land");
            if(hero.KeyDirection==0) return new StateWait_();
            else                     return new StateRun_();
        }

        return this;
    }

    public override void Exit(HeroMover hero)
    {
        //
    }
}
