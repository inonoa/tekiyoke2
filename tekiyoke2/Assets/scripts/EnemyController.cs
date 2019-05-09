using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;

    public HeroMover hero;

    public void MovePos(float vx, float vy){
        rb.MovePosition(new Vector2(
            transform.position.x + vx*Time.timeScale,
            transform.position.y + vy*Time.timeScale
        ));
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePos(-1,0);
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="Player"){
            Tokitome.SetTime(1);
            hero.Damage(1);
        }
    }
}