﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;

public class GameTimeCounter : MonoBehaviour
{
    public static GameTimeCounter CurrentInstance{ get; set; }

    public float Seconds{ get; set; } = 0;
    Text txt;
    public bool DoesTick { get; set; } = true;

    void Awake(){
        CurrentInstance = this;
    }

    void Start()
    {
        txt = GetComponent<Text>();
    }

    void Update()
    {
        int secInt = (int)Seconds;
        int secs    = secInt % 60;
        int minutes = secInt / 60;
        int comma__ = (int) ((Seconds - secInt) * 100);
        txt.text = minutes.ToString("00") + ":" + secs.ToString("00") + ":" + comma__.ToString("00");

        if(DoesTick) Seconds += Time.timeScale * Time.deltaTime;
    }
}
