﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearToEnemyChecker : MonoBehaviour
{
    public HeroMover hero;

    // Start is called before the first frame update
    void Start()
    {
        hero = GetComponentInParent<HeroMover>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Enemy" && hero.dashcntr.State!=DashController.DState.Dashing){
            Tokitome.SetTime(0.1f);
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag=="Enemy"){
            Tokitome.SetTime(1);
        }
    }
}