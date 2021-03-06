﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogeDropController : MonoBehaviour
{
    new Rigidbody2D rigidbody;

    enum State{ Wait, Ready, Drop, Die }
    State state = State.Wait;

    [SerializeField] float dist2FindHeroX = 30;
    [SerializeField] float dist2FindHeroY = 500;
    int readyCount = 0;
    [SerializeField] int readyCountMax = 30;
    [SerializeField] float gravity = 2.5f;
    float velocityY = 0;
    [SerializeField] int frames2Respawn = 100;
    Vector3 defaultPos;
    public Vector3 DefaultPosition{ get => defaultPos; }
    
    void Start(){
        rigidbody = GetComponent<Rigidbody2D>();
        defaultPos = transform.position;
        TogeDropRespawner.Instance.AddDrop(this);
    }

    public void OnRespawn(){
        state = State.Wait;
        readyCount = 0;
        velocityY = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Tags.Hero))  HeroDefiner.currentHero.Damage(1, DamageType.Normal);
        if(other.CompareTag(Tags.Terrain) || other.CompareTag(Tags.SurinukeYuka)) state = State.Die;
    }

    void FixedUpdate()
    {
        switch(state){
            case State.Wait:
                if(System.Math.Abs(transform.position.x - HeroDefiner.CurrentPos.x) < dist2FindHeroX
                    && HeroDefiner.CurrentPos.y < transform.position.y
                    && HeroDefiner.CurrentPos.y > transform.position.y - dist2FindHeroY){
                        state = State.Ready;
                }
                break;

            case State.Ready:
                readyCount ++;
                if(readyCount >= readyCountMax) state = State.Drop;
                break;

            case State.Drop:
                velocityY -= gravity;
                rigidbody.MovePosition(transform.position.ToVec2() + new Vector2(0, velocityY));
                //地面に接触したら死ぬ、はOnTriggerEnter2Dで
                break;

            case State.Die:
                gameObject.SetActive(false);
                TogeDropRespawner.Instance.SendDeath(this, frames2Respawn);
                break;
        }
    }
}
