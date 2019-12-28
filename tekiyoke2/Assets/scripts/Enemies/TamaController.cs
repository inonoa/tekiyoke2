using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TamaController : MonoBehaviour
{
    Rigidbody2D rBody;
    public float angle;

    public float speed;
    Vector3 speedVec;

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
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Terrain" || other.gameObject.tag == "Player") Destroy(gameObject);
        if(other.gameObject.tag == "Player") HeroDefiner.currentHero.Damage(1);
    }
}
