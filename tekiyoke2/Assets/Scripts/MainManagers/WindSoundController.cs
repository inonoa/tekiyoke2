﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class WindSoundController : MonoBehaviour
{
    SoundGroup soundGroup;
    readonly IReadOnlyList<string> windNames = new string[]{"Wind0", "Wind1", "Wind2"};
    [SerializeField] FloatPair[] volumeMinMaxsDependingOnHP = new FloatPair[4];

    IEnumerator changingVolumes;

    [SerializeField] float heroCheckPeriodSec = 0.5f;
    [SerializeField] [Range(0,1)] float actualVal = 0;

    void Start()
    {
        soundGroup = GetComponent<SoundGroup>();
        for(int i=0;i<3;i++){
            int i_ = i;
            DOVirtual.DelayedCall(i_, () => soundGroup.Play(windNames[i_]) );
        }
        changingVolumes = ChangeVolumes();
        StartCoroutine(changingVolumes);
    }

    IEnumerator ChangeVolumes()
    {
        while(true)
        {
            yield return new WaitForSeconds(heroCheckPeriodSec);

            FloatPair vol_hp = volumeMinMaxsDependingOnHP[HeroDefiner.currentHero.HPController.HP];
            Vector3 pastPos = HeroDefiner.PastPoss.Count > 0 ?
                HeroDefiner.PastPoss[(int)(heroCheckPeriodSec * 60)] : new Vector3();
            float vol_speed = 
                (HeroDefiner.CurrentPos - pastPos)
                .magnitude
                / (heroCheckPeriodSec * 60)
                / 20;
            
            float actualVolume = Mathf.Lerp(vol_hp.Min, vol_hp.Max, vol_speed);

            actualVal = actualVolume;

            foreach (var windName in windNames)
            {
                soundGroup.VolumeTo(windName, actualVolume, heroCheckPeriodSec);
            }
        }
    }

    public void FadeOut(float durationSec){
        if(changingVolumes != null) StopCoroutine(changingVolumes);
        soundGroup.FadeoutAll(durationSec);
    }


    void OnDisable() => soundGroup.StopAll();

    void OnEnable(){
        if(changingVolumes!= null) StartCoroutine(changingVolumes);
        if(soundGroup != null) soundGroup.PlayAll();
    }
}

[Serializable]
public class FloatPair{

    [field: SerializeField] [field: RenameField("Min")]
    public float Min{ get; private set; }

    [field: SerializeField] [field: RenameField("Max")]
    public float Max{ get; private set; }
}
