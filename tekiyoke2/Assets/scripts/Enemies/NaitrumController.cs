using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;

public class NaitrumController : EnemyController
{
    private int direction = -1;
    public NaitrumCollider col;

    private void Turn(object sender, EventArgs e) => direction *= -1;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        col = GetComponent<NaitrumCollider>();
        col.turn += Turn;
    }

    // Update is called once per frame
    void Update() => MovePos(direction,0);
}