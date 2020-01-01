﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPCD : MonoBehaviour
{
    static readonly int rotateInterval = 10;
    int rotateCount = 0;
    [SerializeField]
    int DPperDPCD = 1;

    // Update is called once per frame
    void Update()
    {
        rotateCount ++;
        rotateCount %= rotateInterval;
        if(rotateCount==0) transform.Rotate(new Vector3(0,0,45));
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Destroy(gameObject);
            DPManager.Instance.AddDP(DPperDPCD);

            //これは後で消す
            if(DPManager.Instance.DP == 100) GameTimeCounter.CurrentInstance.DoesTick = false;
        }
    }
}
