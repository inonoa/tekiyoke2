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
    private DState state = DState.Off;

    ///<summary>外から読むときはこっち(ダッシュ中かどうか/色々)</summary>
    public DState State{get{return state;}}

    ///<summary>ダッシュの向き</summary>
    public bool dashToRight = false;

    ///<summary>タメ開始してから何F経った？</summary>
    public int tame2dash = 0;

    ///<summary>タメ終了(ダッシュ開始)からダッシュ終了にかかる時間。タメ終了時に毎回セットされる</summary>
    private int dashFullTime;

    ///<summary>タメ終了から何F経過したか？</summary>
    public int dashTime = 0;


    ///<summary>フレームごとの移動距離</summary>
    public float dashX = 0;


    ///<summary>ダッシュボタンを押したときにタメが開始されるか？</summary>
    public bool CanDash{
        get{
            return state==DState.Off;
        }
    }

    public void StandBy(bool dashToRight){
        state = DState.StandingBy;
        this.dashToRight = dashToRight;
    }

    ///<summary>フレームごとの移動距離の配列。タメ終了時にタメの強さに応じていい感じの値が格納される</summary>
    private float[] moveDists;
    //T[F] = X[Unit] * 3/20 くらい

    ///<summary>タメ終了時に呼ぶ。ための強さに応じてmoveDistsに移動距離を格納し、ダッシュ中に遷移。</summary>
    public int ExecuteDash(){
        int x = 500;
        int t = (x*3) /20;
        moveDists = new float[t];
        for(int i=0;i<t;i++){
            moveDists[i] = x * ( IikanjinoKansuu((i+1)/(float)t) - IikanjinoKansuu(i/(float)t) );
        }
        dashFullTime = t;
        int re = tame2dash;
        dashTime = 0;
        tame2dash = 0;
        state = DState.Dashing;
        Tokitome.SetTime(1);
        return re;
    }

    public float IikanjinoKansuu(float t_T){
        return (1-(float)Math.Cos(Math.PI*t_T))/2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // タメ中ならタメる
        if(state==DState.StandingBy){
            tame2dash ++;
        }
        // ダッシュ中なら次の移動距離を準備。
        else if(state==DState.Dashing){
            if(this.dashToRight){dashX = moveDists[dashTime];}
            else{dashX = -moveDists[dashTime];}
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
