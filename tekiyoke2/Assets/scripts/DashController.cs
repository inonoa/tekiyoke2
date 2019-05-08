using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DashController : MonoBehaviour
{

    public enum DState{
        Off, StandingBy, Dashing, InCoolTime,
    }
    public DState state = DState.Off;
    private int dashFullTime;
    public int dashTime = 0;

    float dashX = 0;

    private bool CanDash{
        get{
            return state==DState.Off;
        }
    }


    private float[] moveDists;
    // x = (4999 - 4900*(t/T)^2) * ((t/T)^2 /99) * X (Tはダッシュに要する時間、Xは距離)の曲線に従って移動する。
    //ここで、 T[F] = X[Unit] * 3/20 くらいである。

    void ExecuteDash(float velocity){
        int x = 200;
        int t = (x*3) /20;
        float tt = t^2;
        moveDists = new float[t];
        for(int i=0;i<t;i++){
            if(i!=t-1){moveDists[i+1] = - (4999 - 4900*((i+1)^2)/tt) * (((i+1)^2)/tt /99) * x;}
            moveDists[i] += (4999 - 4900*i*i/tt) * (i*i/tt /99) * x;
        }
        dashFullTime = t;
        dashTime = 0;
        state = DState.Dashing;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state==DState.Dashing){
            dashX = moveDists[dashTime];
            dashTime ++;
            if(dashFullTime==dashTime){
                state = DState.InCoolTime;
            }
        }
        else if(state==DState.InCoolTime){
            dashTime --;
            if(dashTime==0){
                state = DState.Off;
                dashFullTime = 0;
            }
        }
    }
}
