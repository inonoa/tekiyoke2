using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBaneYoko : IHeroState
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

    public void Start(){
        hero.velocity.x = toRight ? pushSpeed : -pushSpeed;
        hero.anim.SetTrigger(toRight ? "runr" : "runl");
        hero.EyeToRight = toRight;
    }

    public void Update(){

        if(unstoppable){
            frames2BeStoppable --;
            if(frames2BeStoppable==0) unstoppable = false;

        }else{
            if(!hero.IsOnGround){
                hero.States.Push(new StateFall(hero));
                hero.speedResidues.Add(new BaneResidue(hero.velocity.x, speedBreak));
            }

            //この辺全部BaneResidueに移管してもいいかもね
            if(toRight){
                switch(hero.KeyDirection){
                    case 1:
                        hero.velocity.x -= speedBreak / 2;
                        //これでは一瞬右に行って状態戻してから左に行くムーブが(引き返すには)最適になるぞ！！！！
                        //RTAで変態挙動出来そうだし(誰がやるの？？)まあいいか……
                        if(hero.velocity.x <= HeroMover.moveSpeed) hero.States.Push(new StateRun(hero));
                        break;

                    case 0:
                        hero.velocity.x -= speedBreak;
                        if(hero.velocity.x <= 0){
                            hero.velocity.x = 0;
                            hero.States.Push(new StateWait(hero));
                        }
                        break;

                    case -1:
                        hero.velocity.x -= speedBreak * 2;
                        if(hero.velocity.x <= 0) hero.States.Push(new StateRun(hero));
                        break;
                }
            }
            else{
                switch(hero.KeyDirection){
                    case 1:
                        hero.velocity.x += speedBreak * 2;
                        //これでは一瞬右に行って状態戻してから左に行くムーブが(引き返すには)最適になるぞ！！！！
                        //RTAで変態挙動出来そうだし(誰がやるの？？)まあいいか……
                        if(hero.velocity.x >= 0) hero.States.Push(new StateRun(hero));
                        break;

                    case 0:
                        hero.velocity.x += speedBreak;
                        if(hero.velocity.x >= 0){
                            hero.velocity.x = 0;
                            hero.States.Push(new StateWait(hero));
                        }
                        break;

                    case -1:
                        hero.velocity.x += speedBreak / 2;
                        if(hero.velocity.x >= -HeroMover.moveSpeed) hero.States.Push(new StateRun(hero));
                        break;
                }
            }
        }
    }

    public void Try2StartJet(){
        if(!unstoppable){
            hero.States.Push(new StateJet(hero));
            hero.speedResidues.Add(new BaneResidue(hero.velocity.x, speedBreak));
        }
    }
    public void Try2EndJet(){ }
    public void Try2Jump(){
        if(!unstoppable){
            hero.States.Push(new StateJump(hero));
            hero.speedResidues.Add(new BaneResidue(hero.velocity.x, speedBreak));
        }
    }
    public void Try2StartMove(bool toRight){ }
    public void Try2EndMove(){ }
    public void Exit(){
        if(hero.KeyDirection != 0) hero.EyeToRight = (hero.KeyDirection == 1);
    }
}
