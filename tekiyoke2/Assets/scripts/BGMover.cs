﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMover : MonoBehaviour
{

    public float bgrate = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(HeroDefiner.CurrentHeroPos.x/(1+bgrate),HeroDefiner.CurrentHeroPos.y/(1+bgrate)+50);
    }
}
