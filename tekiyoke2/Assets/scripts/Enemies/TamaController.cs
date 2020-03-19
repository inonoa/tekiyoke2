using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class TamaController : MonoBehaviour
{
    Rigidbody2D rBody;
    [HideInInspector] public float angle;

    [HideInInspector] public float speed;
    Vector3 speedVec;

    [HideInInspector] public int life;

    float lifeNow;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        speedVec = speed * new Vector3((float)Math.Cos(angle * Math.PI / 180), (float)Math.Sin(angle * Math.PI / 180));
        lifeNow = life;
    }

    void Update()
    {
        rBody.MovePosition(transform.position + speedVec * Time.timeScale);
        lifeNow -= Time.timeScale;
        if(lifeNow <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Terrain" || other.gameObject.tag == "Ultrathin" || other.gameObject.tag == "Player"){
            GetComponent<SpriteRenderer>().DOFade(0, 0.1f);
            DOVirtual.DelayedCall(0.1f, () => Destroy(gameObject));
        }
        if(other.gameObject.tag == "Player") HeroDefiner.currentHero.Damage(1);
    }
}
