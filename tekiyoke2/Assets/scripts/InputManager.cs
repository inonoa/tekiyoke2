using System.Collections;
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

    #endregion


    #region GetDownUp

    public bool GetButton(ButtonCode b)     => buttons4Latency[(int)b].Peek();
    public bool GetButtonDown(ButtonCode b) => buttonsDown4Latency[(int)b] == 0;
    public bool GetButtonUp(ButtonCode b)   => buttonsUp4Latency[(int)b] == 0;


    Queue<bool>[] buttons4Latency = new Queue<bool>[Enum.GetNames(typeof(ButtonCode)).Length];
    int[] buttonsDown4Latency = new int[Enum.GetNames(typeof(ButtonCode)).Length];
    int[] buttonsUp4Latency   = new int[Enum.GetNames(typeof(ButtonCode)).Length];

    #endregion

    public bool ButtonsDownSimultaneously(ButtonCode b1, ButtonCode b2){
        
        if(GetButtonDown(b1) && (buttonsDown4Latency[(int)b2] >= 0)){
            buttonsDown4Latency[(int)b1] = -1;
            buttonsDown4Latency[(int)b2] = -1;
            return true;
        }
        if(GetButtonDown(b2) && (buttonsDown4Latency[(int)b1] >= 0)){
            buttonsDown4Latency[(int)b1] = -1;
            buttonsDown4Latency[(int)b2] = -1;
            return true;
        }
        return false;
    }

    ///<summary>Edit->Project Settings->Script Execution Orderの設定をしたので他のUpdate()より早く実行される</summary>
    void Update(){

        foreach(ButtonCode b in Enum.GetValues(typeof(ButtonCode))){

            while(buttons4Latency[(int)b].Count > inputLatencies[(int)b]){
                buttons4Latency[(int)b].Dequeue();
            }
            buttonsDown4Latency[(int)b] --;
            buttonsUp4Latency[(int)b] --;
        }

        foreach(ButtonCode b in Enum.GetValues(typeof(ButtonCode))){

            foreach(KeyCode k in button2Keys[b]){

                if(buttons4Latency[(int)b].Count <= inputLatencies[(int)b])
                    buttons4Latency[(int)b].Enqueue(Input.GetKey(k));

                if(Input.GetKeyDown(k)) buttonsDown4Latency[(int)b] = inputLatencies[(int)b];
                if(Input.GetKeyUp(k))   buttonsUp4Latency[(int)b]   = inputLatencies[(int)b];
            }
        }
    }

    static Dictionary<ButtonCode, KeyCode[]> button2Keys = new Dictionary<ButtonCode, KeyCode[]>()
    {
        {ButtonCode.Right,  new[]{KeyCode.RightArrow         }},
        {ButtonCode.Left,   new[]{KeyCode.LeftArrow          }},
        {ButtonCode.Up,     new[]{KeyCode.UpArrow            }},
        {ButtonCode.Down,   new[]{KeyCode.DownArrow          }},
        {ButtonCode.Jump,   new[]{KeyCode.UpArrow, KeyCode.W }},
        {ButtonCode.JetLR,  new[]{KeyCode.A, KeyCode.D       }},
        {ButtonCode.JetL,   new[]{KeyCode.A                  }},
        {ButtonCode.JetR,   new[]{KeyCode.D                  }},
        {ButtonCode.Zone,   new[]{KeyCode.Return             }},
        {ButtonCode.Save,   new[]{KeyCode.Space              }},
        {ButtonCode.Pause,  new[]{KeyCode.S                  }},
        {ButtonCode.Enter,  new[]{KeyCode.Z                  }},
        {ButtonCode.Cancel, new[]{KeyCode.X                  }}
    };


    #region Instance

    public static InputManager Instance{ get; private set; }
    public InputManager() => Instance = this;

    #endregion
}

public enum ButtonCode{
    Right, Left, Up, Down,
    Jump,JetL, JetR, JetLR,
    Zone, Save, Pause, Enter, Cancel
}
