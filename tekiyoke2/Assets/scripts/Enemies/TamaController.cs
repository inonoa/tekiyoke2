using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class TamaController : MonoBehaviour, IReusable
{
    Rigidbody2D rBody;
    Vector3 speedVec;
    float lifeNow;
    public bool InUse{ get; private set; }

    public void Activate(string angle_speed_life){
        string[] a_s_l = angle_speed_life.Split();
        float angle = float.Parse(a_s_l[0]);
        transform.rotation = Quaternion.identity;
        transform.Rotate(0,0, angle);
        speedVec = float.Parse(a_s_l[1]) * new Vector3((float)Math.Cos(angle * Math.PI / 180), (float)Math.Sin(angle * Math.PI / 180));
        lifeNow = int.Parse(a_s_l[2]);
        InUse = true;
        GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rBody.MovePosition(transform.position + speedVec * Time.timeScale);
        lifeNow -= Time.timeScale;
        if(lifeNow <= 0) Die();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Terrain" || other.gameObject.tag == "Ultrathin" || other.gameObject.tag == "Player"){
            GetComponent<SpriteRenderer>().DOFade(0, 0.1f);
            DOVirtual.DelayedCall(0.1f, Die);
        }
        if(other.gameObject.tag == "Player") HeroDefiner.currentHero.Damage(1, DamageType.Normal);
    }

    void Die(){
        gameObject.SetActive(false);
        InUse = false;
    }
}
