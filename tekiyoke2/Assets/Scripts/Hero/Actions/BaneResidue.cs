using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaneResidue : ISpeedResidue
{
    readonly bool toRight;
    readonly float firstSpeed;
    readonly float lifeSeconds;
    float secondsToDie;
    public BaneResidue(bool right, float firstSpeed, float lifeSeconds)
    {
        this.toRight = right;
        this.firstSpeed   = firstSpeed;
        this.lifeSeconds  = lifeSeconds;
        this.secondsToDie = lifeSeconds;
    }

    public bool IsActive => _IsActive;
    bool _IsActive = true;
    public Vector2 UpdateVel(Vector2 currentVeclocity, float deltatime, HeroMover hero)
    {
        if(secondsToDie < 0) return currentVeclocity;

        Vector2 ans = currentVeclocity + new Vector2(CurrendAdditionalVelocity(), 0);

        secondsToDie -= deltatime;
        if(secondsToDie < 0) _IsActive = false;
        
        return ans;
    }

    float CurrendAdditionalVelocity()
    {
        float abs = firstSpeed * secondsToDie / lifeSeconds;
        return toRight ? abs : -abs;
    }
}
