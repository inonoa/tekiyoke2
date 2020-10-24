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

    //速度に関するパラメータ
    [SerializeField] float rotateVelPerSecFirst = 500;
    [SerializeField] float secsUntilSlow = 5;
    static readonly float rotatingThresholdPerSec = 100;

    public bool IsRotatingEnough => logRotateVel >= logVelThreshold;

    //内部的なもの
    float logRotateVel;
    float logVelFirst;
    float logVelDecay;
    readonly float logVelThreshold = Mathf.Log(rotatingThresholdPerSec);
    static readonly float logVelEpsilon = 0;

    //おわり

    public event EventHandler Rotated;
    public event EventHandler OnSlow;

    void Start(){
        mat = kurumaSR.material;
        edgeLightVolMax = mat.GetFloat("_Volume");
        mat.SetFloat("_Volume", 0);

        logVelFirst = Mathf.Log(rotateVelPerSecFirst);
        logVelDecay = (logVelFirst - logVelThreshold) / secsUntilSlow;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Wind"){
            logRotateVel = logVelFirst;
            Rotated?.Invoke(this, EventArgs.Empty);
        }
    }

    void FixedUpdate()
    {
        if(logRotateVel > logVelEpsilon)
        {
            float deltaTime = TimeManager.CurrentInstance.DeltaTimeExceptHero;

            float actualVelocity = Mathf.Exp(logRotateVel) * deltaTime;
            kuruma.Rotate(0,0, - actualVelocity );

            if(IsRotatingEnough){
                float velFirst = rotateVelPerSecFirst * deltaTime;
                mat.SetFloat("_Volume", actualVelocity / velFirst * edgeLightVolMax);
            }

            float tmpNextVel = logRotateVel - logVelDecay * deltaTime;

            if(logRotateVel >= logVelThreshold && tmpNextVel < logVelThreshold){
                mat.SetFloat("_Volume", 0);
                OnSlow?.Invoke(this, EventArgs.Empty);
            }

            logRotateVel = tmpNextVel;
        }
    }
}
