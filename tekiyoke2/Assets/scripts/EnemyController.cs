using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    public HeroMover hero;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(new Vector3(transform.position.x-1,transform.position.y));
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="Player"){
            Time.timeScale = 1;
            hero.Damage(1);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Player"){
            Time.timeScale = 0.1f;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag=="Player"){
            Time.timeScale = 1;
        }
    }
}