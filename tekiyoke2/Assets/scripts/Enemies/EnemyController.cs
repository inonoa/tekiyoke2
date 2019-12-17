﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;

public class EnemyController : MonoBehaviour
{
    protected Rigidbody2D rBody;

    [SerializeField]
    protected float fallSpeedMax = 300;

    int groundCount = 0;
    protected bool IsOnGround{
        get{ return groundCount > 0; }
    }

    protected void MovePos(float v_x, float v_y) =>
        rBody.MovePosition(new Vector2(rBody.transform.position.x + v_x*Time.timeScale,
                                       rBody.transform.position.y + v_y*Time.timeScale));

    ///<summary>指定したv_xだけ横に移動する。y軸方向には重力のままに移動する。</summary>
    protected void MoveX_ConsideringGravity(float v_x) =>
        rBody.velocity = new Vector2(v_x * Time.timeScale, Math.Max(rBody.velocity.y, -fallSpeedMax));

    protected void Start() => rBody = GetComponent<Rigidbody2D>();

    protected void Update() => rBody.velocity = new Vector2(0, rBody.velocity.y);
}
