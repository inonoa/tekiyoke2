using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;

public class GameTimeCounter : MonoBehaviour
{
    public static GameTimeCounter CurrentInstance{ get; set; }

    public float count=0;
    Text txt;
    public bool DoesTick { get; set; } = true;

    void Awake(){
        CurrentInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = (((int)count)/3600).ToString("00") + ":" + (((int)count)%3600/60).ToString("00") + ":" + (((int)count)%60).ToString("00");
        if(DoesTick) count += Time.timeScale;
    }
}
