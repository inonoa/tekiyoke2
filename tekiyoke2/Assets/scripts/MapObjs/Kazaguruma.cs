using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Kazaguruma : MonoBehaviour{
    [SerializeField] Transform kuruma;
    [SerializeField] float rotateVelFirst = 0.1f;
    [SerializeField] float nextRotateVelRate = 0.9f;
    [SerializeField] float rotatingThreshold = 0.02f;
    float rotateVel;
    public bool IsRotating{
        get => rotateVel > rotatingThreshold;
    }
    public event EventHandler Rotated;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Wind"){
            rotateVel = rotateVelFirst;
            Rotated?.Invoke(this, EventArgs.Empty);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rotateVel > 0){
            kuruma.Rotate(0,0,rotateVel);
            rotateVel *= nextRotateVelRate;
            if(rotateVel < 0.001f) rotateVel = 0;
        }
    }
}
