using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;
using UniRx;

public class NaitrumController : EnemyController
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] bool toRight = false;
    Collider2Wall col;

    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Animator anim;
    

    private void Turn(object sender, EventArgs e)
    {
        toRight = !toRight;
        foreach(SpriteRenderer sr in spriteRenderers) sr.flipX = toRight;
    }

    void Start()
    {
        base.Init();
        col = GetComponent<Collider2Wall>();
        col.touched2Wall += Turn;
        foreach(SpriteRenderer sr in spriteRenderers) sr.flipX = toRight;

        anim.FollowTimeScale(aroundHero: false);
    }

    new void Update() => MovePos( (toRight ? 1 : -1) * moveSpeed, 0);
}