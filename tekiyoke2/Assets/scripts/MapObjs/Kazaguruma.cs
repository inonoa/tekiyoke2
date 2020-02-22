using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Kazaguruma : MonoBehaviour{
    [SerializeField] Transform kuruma;
    [SerializeField] float rotateVelFirst = 0.1f;
    [SerializeField] float nextRotateVelRate = 0.9f;
    float rotateVel;
    bool rotating = false;
    public bool IsRotating{
        get => rotating;
    }
    public event EventHandler Rotated;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Jet"){
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
