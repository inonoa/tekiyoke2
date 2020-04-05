﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class DPManager : MonoBehaviour
{
    public static DPManager Instance{ get; private set; }

    public int DP{ get; private set; } = 0;
    static readonly int maxDP = 100;



    ///<summary>1Pずつたまっていく感じにしたいので、内部的なDPとは別に見かけのDPを用意してこれをもとに描画</summary>
    int DPonDisplay = 0;
    [SerializeField] float displayInterval = 0.05f;
    float frames2Display = 0.02f;
    Material material;
    [SerializeField] Image uiImage;
    [SerializeField] float lightLifeSeconds = 1;
    float secondsAfterDPChanged = 10;

    Sequence lightSeq;
    Sequence unlightSeq;

    public void AddDP(int delta){
        if(delta > 0){
            DP = Math.Min(maxDP, DP + delta);
        }
        else print("負のDPは得られません");
    }

    public bool UseDP(int dp2Use){
        if(DP >= dp2Use){
            DP -= dp2Use;
            return true;
        }else{
            return false;
        }
    }

    void Awake(){
        Instance = this;
    }
    void Start()
    {
        material = uiImage.material;
    }

    void Update()
    {
        frames2Display -= Time.deltaTime;
        if(frames2Display <= 0){
            frames2Display = displayInterval;

            if(DPonDisplay != DP){
                //急速にDPが増えたら急速に追いついてほしい
                DPonDisplay = (DPonDisplay > DP) ? (DPonDisplay - 1 - (DPonDisplay - DP) / 5) : (DPonDisplay + 1 + (DP - DPonDisplay) / 5);
                material.SetFloat("_WidthNormalized", DPonDisplay / (float)maxDP);
                LightGauge();
                secondsAfterDPChanged = 0;
            }
        }

        secondsAfterDPChanged += Time.deltaTime;
        if(secondsAfterDPChanged >= lightLifeSeconds){
            UnlightGauge();
        }
    }

    void LightGauge(){
        if(unlightSeq!=null && unlightSeq.IsPlaying()) unlightSeq.Pause();
        
        if(lightSeq==null || !lightSeq.IsPlaying()){
            lightSeq = DOTween.Sequence();
            lightSeq.Append(DOTween.To(() => material.GetFloat("_Light"),
                                       lt => material.SetFloat("_Light", lt),
                                       1, 0.2f))
                                       .SetEase(Ease.InOutSine);
        }
    }

    void UnlightGauge(){
        if(lightSeq!=null && lightSeq.IsPlaying()) lightSeq.Pause();

        if(unlightSeq==null || !unlightSeq.IsPlaying()){
            unlightSeq = DOTween.Sequence();
            unlightSeq.Append(DOTween.To(() => material.GetFloat("_Light"),
                                         lt => material.SetFloat("_Light", lt),
                                         0, 0.2f))
                                         .SetEase(Ease.InOutSine);
        }
    }
}
