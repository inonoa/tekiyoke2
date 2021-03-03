using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Sirenix.OdinInspector;

public class TamaController : MonoBehaviour, IReusable
{
    Rigidbody2D rBody;
    [SerializeField, ReadOnly] Vector3 speedVec;
    [SerializeField, ReadOnly] float lifeNow;
    public bool InUse{ get; private set; }

    public void Activate(string angle_speed_life)
    {
        string[] a_s_l = angle_speed_life.Split();
        float angle = float.Parse(a_s_l[0]);
        transform.rotation = Quaternion.identity;
        transform.Rotate(0,0, angle);
        speedVec = float.Parse(a_s_l[1]) * new Vector3((float)Math.Cos(angle * Math.PI / 180), (float)Math.Sin(angle * Math.PI / 180));
        lifeNow = float.Parse(a_s_l[2]);
        InUse = true;
        GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rBody.MovePosition(transform.position + speedVec * TimeManager.Current.FixedDeltaTimeExceptHero);
    }

    void Update()
    {
        lifeNow -= TimeManager.Current.DeltaTimeExceptHero;
        if(lifeNow <= 0) Die();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Tags.Terrain) || other.CompareTag(Tags.Hero) || other.CompareTag(Tags.SurinukeYuka))
        {
            GetComponent<SpriteRenderer>().DOFade(0, 0.1f);
            DOVirtual.DelayedCall(0.1f, Die, ignoreTimeScale: false);
        }
        if(other.CompareTag(Tags.Hero)) HeroDefiner.currentHero.Damage(1, DamageType.Normal);
    }

    void Die()
    {
        gameObject.SetActive(false);
        InUse = false;
    }
}
