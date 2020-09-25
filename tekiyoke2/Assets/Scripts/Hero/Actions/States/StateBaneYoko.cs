using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBaneYoko : HeroState
{
    static readonly float speedBreak = 0.5f;
    bool unstoppable = true;
    int frames2BeStoppable;
    readonly bool toRight;
    readonly float pushSpeed;
    readonly HeroMover hero;

    public StateBaneYoko(HeroMover hero, bool toRight, float pushSpeed = 100, int frames2BeStoppable = 20){
        this.hero = hero;
        this.toRight = toRight;
        this.pushSpeed = pushSpeed;
        this.frames2BeStoppable = frames2BeStoppable;
    }

    public override void Start(){
        hero.velocity.X = toRight ? pushSpeed : -pushSpeed;
        hero.Anim.SetTrigger(toRight ? "runr" : "runl");
        //hero.EyeToRight = toRight;
    }

    public override void Resume(){
        hero.Anim.SetTrigger(toRight ? "runr" : "runl");
    }

    public override void Update(){

        if(unstoppable){
            frames2BeStoppable --;
            if(frames2BeStoppable==0) unstoppable = false;

        }else{
            if(!hero.IsOnGround){
                hero.States.Push(new StateFall(hero));
                //hero.speedResidues.Add(new BaneResidue(hero.velocity.X, speedBreak));
            }

            //この辺全部BaneResidueに移管してもいいかもね
            if(toRight){
                switch(hero.KeyDirection){
                    case 1:
                        hero.velocity.X -= speedBreak / 2;
                        //これでは一瞬右に行って状態戻してから左に行くムーブが(引き返すには)最適になるぞ！！！！
                        //RTAで変態挙動出来そうだし(誰がやるの？？)まあいいか……
                        if(hero.velocity.X <= HeroMover.moveSpeed) hero.States.Push(new StateRun(hero));
                        break;

                    case 0:
                        hero.velocity.X -= speedBreak;
                        if(hero.velocity.X <= 0){
                            hero.velocity.X = 0;
                            hero.States.Push(new StateWait(hero));
                        }
                        break;

                    case -1:
                        hero.velocity.X -= speedBreak * 2;
                        if(hero.velocity.X <= 0) hero.States.Push(new StateRun(hero));
                        break;
                }
            }
            else{
                switch(hero.KeyDirection){
                    case 1:
                        hero.velocity.X += speedBreak * 2;
                        //これでは一瞬右に行って状態戻してから左に行くムーブが(引き返すには)最適になるぞ！！！！
                        //RTAで変態挙動出来そうだし(誰がやるの？？)まあいいか……
                        if(hero.velocity.X >= 0) hero.States.Push(new StateRun(hero));
                        break;

                    case 0:
                        hero.velocity.X += speedBreak;
                        if(hero.velocity.X >= 0){
                            hero.velocity.X = 0;
                            hero.States.Push(new StateWait(hero));
                        }
                        break;

                    case -1:
                        hero.velocity.X += speedBreak / 2;
                        if(hero.velocity.X >= -HeroMover.moveSpeed) hero.States.Push(new StateRun(hero));
                        break;
                }
            }
        }
    }

    public override void Try2StartJet(){
        if(!unstoppable){
            hero.States.Push(new StateJet(hero));
            //hero.speedResidues.Add(new BaneResidue(hero.velocity.X, speedBreak));
        }
    }
    public override void Try2EndJet(){ }
    public override void Try2Jump(){
        if(!unstoppable){
            hero.States.Push(new StateJump(hero));
            //hero.speedResidues.Add(new BaneResidue(hero.velocity.X, speedBreak));
        }
    }
    public override void Try2StartMove(bool toRight){ }
    public override void Try2EndMove(){ }
    public override void Exit(){
        if(hero.KeyDirection != 0);//hero.EyeToRight = (hero.KeyDirection == 1);
    }
}
