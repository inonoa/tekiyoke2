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

    public void Play(string soundName){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].Name == soundName){
                ses[i].Play();
                return;
            }
        }
        Debug.LogError("そんなSEはない");
    }

    public void Stop(string soundName){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].Name == soundName){
                ses[i].Stop();
                return;
            }
        }
        Debug.LogError("そんなSEはない");
    }

    public void StopAll(){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].RequireComponent){
                ses[i].Stop();
            }
        }
    }

    public void FadeOut(string soundName, float durationSec){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].Name == soundName){
                DOTween.To(() => ses[i].Volume, v => ses[i].Volume = v, 0, durationSec);
                return;
            }
        }
        Debug.LogError("そんなSEはない");
    }

    public void VolumeTo(string soundName, float endValue, float durationSec){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].Name == soundName){
                DOTween.To(() => ses[i].Volume, v => ses[i].Volume = v, endValue, durationSec);
                return;
            }
        }
        Debug.LogError("そんなSEはない");
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
