﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCollider2Enemy : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Enemy" && HeroDefiner.currentHero.dashcntr.State!=DashController.DState.Dashing){
            Tokitome.SetTime(1);
            HeroDefiner.currentHero.Damage(1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
