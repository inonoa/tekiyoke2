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

    [SerializeField] SpriteRenderer[] spriteRenderers;
    

    private void Turn(object sender, EventArgs e){
        toRight = !toRight;
        foreach(SpriteRenderer sr in spriteRenderers) sr.flipX = toRight;
    }

    new void Start()
    {
        base.Start();
        col = GetComponent<Collider2Wall>();
        col.touched2Wall += Turn;
        foreach(SpriteRenderer sr in spriteRenderers) sr.flipX = toRight;
    }

    new void Update() => MovePos( (toRight ? 1 : -1) * moveSpeed, 0);
}