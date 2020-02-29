using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Tokitome : MonoBehaviour
{
    public static void SetTime(float rate){
        Time.timeScale = rate;
        Time.fixedDeltaTime = 0.02f * rate;
    }
}
