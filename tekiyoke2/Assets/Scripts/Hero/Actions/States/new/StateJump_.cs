using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump_ : HeroStateBase
{
    float jumpForce = 30f;
    float gravity = 80f;
    float horizontalMoveForce = 150f;
    float horizontalMoveSpeed = 15f;

    public override void Enter(HeroMover hero)
    {
        hero.velocity.Y = jumpForce;
        hero.Rigidbody.velocity = new Vector2(0, 1);
        hero.SetAnim("jumpu");
    }
    public override void Resume(HeroMover hero)
    {
        hero.SetAnim("jumpu");
    }

    public override HeroStateBase HandleInput(HeroMover hero, IAskedInput input)
    {
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
