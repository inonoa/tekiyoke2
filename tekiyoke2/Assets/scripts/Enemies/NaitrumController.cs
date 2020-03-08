using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;

public class NaitrumController : EnemyController
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] bool toRight = false;
    EnemyCollider2Wall col;

    [SerializeField] SpriteRenderer spriteRenderer;
    

    private void Turn(object sender, EventArgs e){
        toRight = !toRight;
        spriteRenderer.flipX = toRight;
    }

    new void Start()
    {
        base.Start();
        col = GetComponent<EnemyCollider2Wall>();
        col.touched2Wall += Turn;
        spriteRenderer.flipX = toRight;
    }

    new void Update() => MovePos( (toRight ? 1 : -1) * moveSpeed, 0);
}