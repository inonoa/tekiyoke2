﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{

    #region latencies

    ///<summary>入力の反映が遅れる代わりに同時押し判定が緩みます</summary>
    int[] inputLatencies = new int[Enum.GetNames(typeof(ButtonCode)).Length];
    public void SetInputLatency(ButtonCode b, int latency) => inputLatencies[(int)b] = latency;
    public int GetInputLatency(ButtonCode b) => inputLatencies[(int)b];

    ///<summary>セットすると全Buttonのはじめの遅延がこの値になる</summary>
    [SerializeField] int defaultLatency = 0;

    #endregion


    #region GetDownUp

    public bool GetButton(ButtonCode b)     => buttons4Latency[(int)b].Peek();
    public bool GetButtonDown(ButtonCode b) => buttonsDown4Latency[(int)b] == 0;
    public bool GetButtonUp(ButtonCode b)   => buttonsUp4Latency[(int)b] == 0;


    Queue<bool>[] buttons4Latency = new Queue<bool>[Enum.GetNames(typeof(ButtonCode)).Length];
    int[] buttonsDown4Latency = new int[Enum.GetNames(typeof(ButtonCode)).Length];
    int[] buttonsUp4Latency   = new int[Enum.GetNames(typeof(ButtonCode)).Length];

    #endregion

    #region その他便利関数

    ///<summary>二つのButtonが同時に押されているか、latencyの分だけ誤差が許容されます</summary>
    public bool ButtonsDownSimultaneously(ButtonCode b1, ButtonCode b2){

        if(GetButtonDown(b1) && (buttonsDown4Latency[(int)b2] >= 0)) return true;
        if(GetButtonDown(b2) && (buttonsDown4Latency[(int)b1] >= 0)) return true;
        return false;
    }

    ///<summary>何らかのButtonに登録されているキーが押されたFにtrue</summary>
    public bool AnyButtonDown(){

        foreach(ButtonCode b in Enum.GetValues(typeof(ButtonCode)))
            if(GetButtonDown(b)) return true;
        return false;
    }

    #endregion

    ///<summary>現在のインスタンスが入るはず(シーンごとに入れ替わる)</summary>
    public static InputManager Instance{ get; private set; }

    void Start(){
        Instance = this;
        foreach(ButtonCode b in Enum.GetValues(typeof(ButtonCode))){
            buttons4Latency[(int)b] = new Queue<bool>();
            inputLatencies[(int)b]  = defaultLatency;
        }
    }

    ///<summary>Edit->Project Settings->Script Execution Orderの設定をしたので他のUpdate()より早く実行される</summary>
    void Update(){

        //もう見た入力をQueueから出す/カウントダウン
        foreach(ButtonCode b in Enum.GetValues(typeof(ButtonCode))){

            while(buttons4Latency[(int)b].Count > inputLatencies[(int)b]){
                buttons4Latency[(int)b].Dequeue();
            }
            buttonsDown4Latency[(int)b] --;
            buttonsUp4Latency[(int)b] --;
        }

        //取ってきた入力を一旦Queueに入れる/カウントダウンを始めることで遅延させる
        foreach(ButtonCode b in Enum.GetValues(typeof(ButtonCode))){

            bool bBeingPushed = false;

            foreach(KeyCode k in button2Keys[b]){

                bBeingPushed |= Input.GetKey(k);

                if(Input.GetKeyDown(k)) buttonsDown4Latency[(int)b] = inputLatencies[(int)b];
                if(Input.GetKeyUp(k))   buttonsUp4Latency[(int)b]   = inputLatencies[(int)b];
            }

            buttons4Latency[(int)b].Enqueue(bBeingPushed);
        }
    }

    ///<summary>ButtonCodeとKeyCode群との対応を書く。入っているKeyのうちいずれかが押されると押された判定になる</summary>
    static Dictionary<ButtonCode, KeyCode[]> button2Keys = new Dictionary<ButtonCode, KeyCode[]>()
    {
        {ButtonCode.Right,  new[]{KeyCode.D, KeyCode.RightArrow }},
        {ButtonCode.Left,   new[]{KeyCode.A, KeyCode.LeftArrow }},
        {ButtonCode.Up,     new[]{KeyCode.UpArrow, KeyCode.W }},
        {ButtonCode.Down,   new[]{KeyCode.DownArrow, KeyCode.S, KeyCode.K }},

        {ButtonCode.Jump,   new[]{KeyCode.UpArrow, KeyCode.W, KeyCode.I }},
        {ButtonCode.JetLR,  new[]{KeyCode.J, KeyCode.L }},
        {ButtonCode.JetL,   new[]{KeyCode.L }},
        {ButtonCode.JetR,   new[]{KeyCode.J }},
        {ButtonCode.Zone,   new[]{KeyCode.Return, KeyCode.Z }},
        {ButtonCode.Save,   new[]{KeyCode.Space, KeyCode.Return }},

        {ButtonCode.Pause,  new[]{KeyCode.Backspace, KeyCode.Escape }},
        {ButtonCode.Enter,  new[]{KeyCode.Return, KeyCode.Z, KeyCode.F1 }},
        {ButtonCode.Cancel, new[]{KeyCode.X, KeyCode.Backspace, KeyCode.F2 }}
    };
}

public enum ButtonCode{
    Right, Left, Up, Down,
    Jump, JetL, JetR, JetLR,
    Zone, Save, Pause, Enter, Cancel
}
