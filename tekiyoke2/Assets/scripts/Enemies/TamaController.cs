using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TamaController : MonoBehaviour
{
    Rigidbody2D rBody;
    [HideInInspector] public float angle;

    [HideInInspector] public float speed;
    Vector3 speedVec;

    [HideInInspector] public int life;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        speedVec = speed * new Vector3((float)Math.Cos(angle * Math.PI / 180), (float)Math.Sin(angle * Math.PI / 180));
    }

    // Update is called once per frame
    void Update()
    {
        rBody.MovePosition(transform.position + speedVec);
        life --;
        if(life==0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Terrain" || other.gameObject.tag == "Ultrathin" || other.gameObject.tag == "Player") Destroy(gameObject);
        if(other.gameObject.tag == "Player") HeroDefiner.currentHero.Damage(1);
    }
}
