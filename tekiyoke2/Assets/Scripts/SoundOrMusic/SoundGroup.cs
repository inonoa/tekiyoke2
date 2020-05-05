using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using DG.Tweening;

public class SoundGroup : MonoBehaviour
{
    [SerializeField] SoundEffect[] ses;

    void Start(){
        AudioSource source4SE = gameObject.AddComponent<AudioSource>();

        foreach(SoundEffect se in ses){
            if(se.RequireComponent){
                se.Initialize(gameObject.AddComponent<AudioSource>());
            }else{
                se.Initialize(source4SE);
            }
        }
    }

    SoundEffect Find(string name){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].Name == name){
                return ses[i];
            }
        }
        Debug.LogError("そんなSEはない");
        return null;
    }

    public void Play(string soundName){
        Find(soundName)?.Play();
    }
    public void PlayAll(){
        for(int i=0; i<ses.Length; i++){
            ses[i].Play();
        }
    }

    public void Stop(string soundName){
        Find(soundName)?.Stop();
    }

    public void StopAll(){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].RequireComponent){
                ses[i].Stop();
            }
        }
    }

    public void SetVolume(string soundName, float volume){
        SoundEffect se = Find(soundName);
        if(se != null) se.Volume = volume;
    }

    public void FadeOut(string soundName, float durationSec){
        SoundEffect se = Find(soundName);
        if(se != null) DOTween.To(() => se.Volume, v => se.Volume = v, 0, durationSec);
    }

    public void VolumeTo(string soundName, float endValue, float durationSec){
        SoundEffect se = Find(soundName);
        if(se != null) DOTween.To(() => se.Volume, v => se.Volume = v, endValue, durationSec);
    }
}

[Serializable]
public class SoundEffect{

    [SerializeField] string _Name;
    public string Name => _Name;

    [SerializeField] AudioClip _Clip;
    public AudioClip Clip => _Clip;

    [SerializeField] [Range(0, 1)] float _Volume = 1;
    public float Volume{
        get => _Volume;
        set{
            _Volume = Mathf.Clamp01(value);
            source.volume = _Volume;
        }
    }

    [SerializeField] [FormerlySerializedAs("_CanLoopAndStop")] bool _RequireComponent = false;
    public bool RequireComponent => _RequireComponent;


    [field: SerializeField] [field: RenameField("Loop")]
    public bool Loop{ get; private set; }
    

    AudioSource source;

    public void Initialize(AudioSource source){
        this.source = source;
        source.playOnAwake = false;
        if(RequireComponent){
            source.volume = Volume;
            source.clip = Clip;
            source.loop = Loop;
        }
    }

    public void Play(){
        if(RequireComponent){
            source.Play();
        }else{
            source.PlayOneShot(Clip, Volume);
        }
    }

    public void Stop(){
        if(RequireComponent){
            source.Stop();
        }
        else Debug.LogError("止まらない！！");
    }
}
