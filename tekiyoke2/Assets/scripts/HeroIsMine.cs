﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIsMine : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            this.transform.position = new Vector3(0,-1500);
        }
    }
}
