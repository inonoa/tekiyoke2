using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaneResidue : ISpeedResidue
{
    public float SpeedX{ get; private set; }
    public float SpeedY{ get; } = 0;

    readonly bool toRight;
    readonly float breakX;

    public BaneResidue(float vxNow, float breakX){
        this.toRight = vxNow > 0;
        if(toRight) SpeedX = vxNow - HeroMover.moveSpeed;
        else        SpeedX = vxNow + HeroMover.moveSpeed;
        this.breakX = breakX;
    }

    public bool UpdateSpeed(HeroMover hero){

        if(toRight){
            switch(hero.KeyDirection){
                case 1 : SpeedX -= breakX / 2; break;
                case 0 : SpeedX -= breakX    ; break;
                case -1: SpeedX -= breakX * 2; break;
            }
            if(SpeedX < 0) return true;
        }
        else{
            switch(hero.KeyDirection){
                case 1 : SpeedX += breakX * 2; break;
                case 0 : SpeedX += breakX    ; break;
                case -1: SpeedX += breakX / 2; break;
            }
            if(SpeedX > 0) return true;
        }
        return false;
    }
}
