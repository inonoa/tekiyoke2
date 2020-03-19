using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Kazaguruma : MonoBehaviour{
    [SerializeField] Transform kuruma;
    [SerializeField] SpriteRenderer kurumaSR;
    Material mat;
    float edgeLightVolMax;
    [SerializeField] float rotateVelFirst = 0.1f;
    [SerializeField] float nextRotateVelRate = 0.9f;
    [SerializeField] float rotatingThreshold = 0.02f;
    float rotateVel;
    public bool IsRotating{
        get => rotateVel > rotatingThreshold;
    }
    public event EventHandler Rotated;
    public event EventHandler OnSlow;

    void Start(){
        mat = kurumaSR.material;
        edgeLightVolMax = mat.GetFloat("_Volume");
        mat.SetFloat("_Volume", 0);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Wind"){
            rotateVel = rotateVelFirst;
            Rotated?.Invoke(this, EventArgs.Empty);
        }
    }

    void Update()
    {
        if(rotateVel > 0){

            kuruma.Rotate(0,0,-rotateVel);

            float nextVel = rotateVel * nextRotateVelRate;

            if(rotateVel >= rotatingThreshold && nextVel < rotatingThreshold){
                mat.SetFloat("_Volume", 0);
                OnSlow?.Invoke(this, EventArgs.Empty);
            }

            rotateVel = nextVel;
            if(rotateVel >= rotatingThreshold) mat.SetFloat("_Volume", rotateVel / rotateVelFirst * edgeLightVolMax);
            if(rotateVel < 0.1f){
                rotateVel = 0;
            }
        }
    }
}
