using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Tokitome : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetTime(float rate){
        Time.timeScale = rate;
        Time.fixedDeltaTime = 0.02f * rate;
        Debug.Log(Time.fixedDeltaTime);
    }
}
