﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using NUnit.Framework;
using Sirenix.OdinInspector;

public class DPManager : MonoBehaviour
{
    public static DPManager Instance{ get; private set; }

    [field: SerializeField, ReadOnly, LabelText(nameof(DP))]
    public float DP{ get; private set; } = 0;
    const int maxDP = 100;

#if UNITY_EDITOR
    [SerializeField, UnityEngine.Range(0, maxDP)] float debugInitialDP = 0;
#endif

    ///<summary>1Pずつたまっていく感じにしたいので、内部的なDPとは別に見かけのDPを用意してこれをもとに描画</summary>
    float DPonDisplay = 0;
    [SerializeField] float displaySpeed = 0.05f;
    Material material;
    [SerializeField] Image uiImage;
    [SerializeField] float lightLifeSeconds = 1;
    float secondsAfterDPChanged = 10;

    Sequence lightSeq;
    Sequence unlightSeq;

    public void AddDP(float delta)
    {
        if(delta > 0)
        {
            DP = Math.Min(maxDP, DP + delta);
        }
        else print("負のDPは得られません");
    }

    public bool UseDP(float dp2Use)
    {
        if(DP >= dp2Use)
        {
            DP -= dp2Use;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ForceUseDP(float dp2Use)
    {
        if(DP >= dp2Use)
        {
            DP -= dp2Use;
            return true;
        }
        else
        {
            DP = 0;
            return false;
        }
    }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        material = uiImage.material;
        material.SetFloat("_WidthNormalized", 0);
        
        if(debugInitialDP > 0) AddDP(debugInitialDP);
    }

    void Update()
    {
        if(DPonDisplay != DP)
        {
            //急速にDPが増えたら急速に追いついてほしい
            float dpDelta = (DPonDisplay > DP) ? - 1 - (DPonDisplay - DP) / 5 : 1 + (DP - DPonDisplay) / 5;
            dpDelta *= displaySpeed;
            dpDelta = dpDelta > 0 ? Mathf.Min(dpDelta, DP - DPonDisplay) : Mathf.Max(dpDelta, DP - DPonDisplay);
            DPonDisplay = DPonDisplay + dpDelta;
            material.SetFloat("_WidthNormalized", DPonDisplay / (float)maxDP);
            LightGauge();
            secondsAfterDPChanged = 0;
        }

        secondsAfterDPChanged += TimeManager.Current.DeltaTimeAroundHero;
        if(secondsAfterDPChanged >= lightLifeSeconds)
        {
            UnlightGauge();
        }
    }

    void LightGauge()
    {
        if(unlightSeq!=null && unlightSeq.IsActive() && unlightSeq.IsPlaying()) unlightSeq.Pause();
        
        if(lightSeq==null || !lightSeq.IsActive() || !lightSeq.IsPlaying())
        {
            lightSeq = DOTween.Sequence();
            lightSeq.Append(DOTween.To(() => material.GetFloat("_Light"),
                                       lt => material.SetFloat("_Light", lt),
                                       1, 0.2f))
                                       .SetEase(Ease.InOutSine);
        }
    }

    public void LightGaugePulse()
    {
        material.SetFloat("_Light", 1);
    }

    void UnlightGauge()
    {
        if(lightSeq!=null && lightSeq.IsActive() && lightSeq.IsPlaying()) lightSeq.Pause();

        if(unlightSeq==null || !unlightSeq.IsActive() || !unlightSeq.IsPlaying())
        {
            unlightSeq = DOTween.Sequence();
            unlightSeq.Append(DOTween.To(() => material.GetFloat("_Light"),
                                         lt => material.SetFloat("_Light", lt),
                                         0, 0.2f))
                                         .SetEase(Ease.InOutSine);
        }
    }
}
