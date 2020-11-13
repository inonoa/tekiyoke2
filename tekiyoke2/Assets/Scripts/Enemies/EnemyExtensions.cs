using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnemyExtensions
{
    public static void MoveX_ConsideringGravity(this Rigidbody2D rBody, float v_x, float fallSpeedMax = 300)
    {
        rBody.velocity = new Vector2(v_x * TimeManager.Current.TimeScaleExceptHero, Math.Max(rBody.velocity.y, -fallSpeedMax));
    }
}
