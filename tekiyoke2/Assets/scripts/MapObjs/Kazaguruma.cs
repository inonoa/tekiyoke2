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
    public event EventHandler OnSlow;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Wind"){
            rotateVel = rotateVelFirst;
            Rotated?.Invoke(this, EventArgs.Empty);
        }
    }

    void Update()
    {
        if(rotateVel > 0){

            kuruma.Rotate(0,0,rotateVel);

            float nextVel = rotateVel * nextRotateVelRate;

            if(rotateVel >= rotatingThreshold && nextVel < rotatingThreshold){
                OnSlow?.Invoke(this, EventArgs.Empty);
            }

            rotateVel = nextVel;
            if(rotateVel < 0.1f){
                rotateVel = 0;
            }
        }
    }
}
