using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UniRx;

public class Tokitome : MonoBehaviour
{
    public static void SetTime(float rate){
        Time.timeScale = rate;
        Time.fixedDeltaTime = 0.02f * rate;
    }

    void Start()
    {
        //
    }

    void Update()
    {
        //
    }
}
