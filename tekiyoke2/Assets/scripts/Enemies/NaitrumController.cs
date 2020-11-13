using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;

public class NaitrumController : EnemyController
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] bool toRight = false;
    Collider2Wall col;

    [Space(10)]
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Rigidbody2D RigidBody;
    

    private void Turn(object sender, EventArgs e){
        toRight = !toRight;
        foreach(SpriteRenderer sr in spriteRenderers) sr.flipX = toRight;
    }

    void Start()
    {
        base.Init();
        col = GetComponent<Collider2Wall>();
        col.touched2Wall += Turn;
        foreach(SpriteRenderer sr in spriteRenderers) sr.flipX = toRight;
    }

    void Update() => MovePos( (toRight ? 1 : -1) * moveSpeed, 0);

    void MovePos(float v_x, float v_y)
    {
        RigidBody.MovePosition(new Vector2(RigidBody.transform.position.x + v_x * TimeManager.CurrentInstance.TimeScaleExceptHero,
                                       RigidBody.transform.position.y + v_y * TimeManager.CurrentInstance.TimeScaleExceptHero));
    }
}