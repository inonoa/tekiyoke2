using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DashController : MonoBehaviour
{

    ///<summary>ダッシュの状態を表すenum</summary>
    public enum DState{
        Off, StandingBy, Dashing, InCoolTime,
    }

    ///<summary>ダッシュの状態を表す</summary>
    public DState state = DState.Off;

    ///<summary>タメ終了(ダッシュ開始)からダッシュ終了にかかる時間。タメ終了時に毎回セットされる</summary>
    private int dashFullTime;

    ///<summary>タメ終了から何F経過したか？</summary>
    public int dashTime = 0;


    ///<summary>フレームごとの移動距離</summary>
    float dashX = 0;


    ///<summary>ダッシュボタンを押したときにタメが開始されるか？</summary>
    private bool CanDash{
        get{
            return state==DState.Off;
        }
    }

    ///<summary>フレームごとの移動距離の配列。タメ終了時にタメの強さに応じていい感じの値が格納される</summary>
    private float[] moveDists;
    // x = (4999 - 4900*(t/T)^2) * ((t/T)^2 /99) * X (Tはダッシュに要する時間、Xは距離)の曲線に従って移動する。
    //ここで、 T[F] = X[Unit] * 3/20 くらいである。

    ///<summary>タメ終了時に呼ぶ。ための強さに応じてmoveDistsに移動距離を格納し、ダッシュ中に遷移。</summary>
    ///<param name="velocity">タメの強さ(って何)</param>
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
        // ダッシュ中なら次の移動距離を準備。
        if(state==DState.Dashing){
            dashX = moveDists[dashTime];
            dashTime ++;
            if(dashFullTime==dashTime){
                state = DState.InCoolTime;
            }
        }
        // ダッシュ後なら残りcool timeを減らす
        else if(state==DState.InCoolTime){
            dashTime --;
            if(dashTime==0){
                state = DState.Off;
                dashFullTime = 0;
            }
        }
    }
}
