using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;

public class EnemyController : MonoBehaviour
{
    private int direction = -1;
    Rigidbody2D rb;
    public EnemyCollider col;


    public void MovePos(float vx, float vy){
        rb.MovePosition(new Vector2(
            transform.position.x + vx*Time.timeScale,
            transform.position.y + vy*Time.timeScale
        ));
    }

    private void Turn(object sender, EventArgs e){
        direction *= -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col.turn += Turn;
    }

    // Update is called once per frame
    void Update()
    {
        MovePos(direction,0);
    }
}