using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;

public class NaitrumController : EnemyController
{
    private int direction = -1;
    public EnemyCollider2Wall col;

    private void Turn(object sender, EventArgs e) => direction *= -1;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        col = GetComponent<EnemyCollider2Wall>();
        col.touched2Wall += Turn;
    }

    // Update is called once per frame
    new void Update() => MovePos(direction,0);
}