using System.Collections;
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

    IEnumerator ChangeVolumes(){
        while(true){
            yield return new WaitForSeconds(heroCheckPeriodSec);

            FloatPair vol_hp = volumeMinMaxsDependingOnHP[HeroDefiner.currentHero.HpCntr.HP];
            float vol_speed = 
                (HeroDefiner.CurrentHeroPos - HeroDefiner.CurrentHeroPastPos[(int)(heroCheckPeriodSec * 60)])
                .magnitude
                / (heroCheckPeriodSec * 60)
                / 20;
            
            float actualVolume = Mathf.Lerp(vol_hp.Min, vol_hp.Max, vol_speed);

            actualVal = actualVolume;

            for(int i=0;i<3;i++){
                soundGroup.VolumeTo(windNames[i], actualVolume, heroCheckPeriodSec);
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
