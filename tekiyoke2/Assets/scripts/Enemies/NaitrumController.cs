using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;

public class NaitrumController : EnemyController
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] bool toRight = false;
    public EnemyCollider2Wall col;

    private void Turn(object sender, EventArgs e) => toRight = !toRight;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        col = GetComponent<EnemyCollider2Wall>();
        col.touched2Wall += Turn;
    }

    // Update is called once per frame
    new void Update() => MovePos( (toRight ? 1 : -1) * moveSpeed, 0);
}