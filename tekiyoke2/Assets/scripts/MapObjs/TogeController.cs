﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogeController : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag=="Player") HeroDefiner.currentHero.Damage(3);
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
