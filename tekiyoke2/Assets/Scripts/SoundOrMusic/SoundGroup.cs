using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using DG.Tweening;
using UnityEngine.Audio;

public class SoundGroup : MonoBehaviour
{
    [SerializeField] SoundEffect[] ses;
    
    [SerializeField] AudioMixerGroup SEGroup;
    [SerializeField] AudioMixerGroup BGMGroup;
    
    bool[] wasPlayingLastMoment;
    
    Action[] finished;
    
    [field: SerializeField, RenameField(nameof(IsSEs))]
    public bool IsSEs { get; private set; } = true;

    void Start()
    {
        Debug.Log(gameObject.name);
        Debug.Assert(SEGroup.name == "SE" && BGMGroup.name == "BGM");
        
        AudioSource source4SE = gameObject.AddComponent<AudioSource>();
        source4SE.outputAudioMixerGroup = IsSEs ? SEGroup : BGMGroup;

        foreach(SoundEffect se in ses)
        {
            if (se.RequireComponent)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = IsSEs ? SEGroup : BGMGroup;
                se.Initialize(source);
            }
            else
            {
                se.Initialize(source4SE);
            }
        }

        wasPlayingLastMoment = new bool[ses.Length];
        finished = new Action[ses.Length];
    }

    void Update(){
        for(int i=0; i<ses.Length; i++){
            if(wasPlayingLastMoment[i] && !ses[i].IsPlaying){
                finished[i]?.Invoke();
                finished[i] = null;
            }
            wasPlayingLastMoment[i] = ses[i].IsPlaying;
        }
    }

    int Find(string name){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].Name == name){
                return i;
            }
        }
        Debug.LogError("そんなSEはない");
        return -1;
    }

    public void Play(string soundName, Action callBack = null){
        int idx = Find(soundName);
        if(idx>-1){
            ses[idx].Play();
            wasPlayingLastMoment[idx] = true;
            finished[idx] += callBack;
        }
    }
    public void PlayAll(){
        for(int i=0; i<ses.Length; i++){
            ses[i].Play();
            wasPlayingLastMoment[i] = true;
        }
    }

    public void Stop(string soundName){
        int idx = Find(soundName);
        if(idx > -1 && ses[idx].RequireComponent){
            ses[idx].Stop();
            wasPlayingLastMoment[idx] = false;
        }
    }

    public void StopAll(){
        for(int i=0; i<ses.Length; i++){
            if(ses[i].RequireComponent){
                ses[i].Stop();
                wasPlayingLastMoment[i] = false;
            }
        }
    }

    public void SetVolume(string soundName, float volume){
        int idx = Find(soundName);
        if(idx > -1) ses[idx].Volume = volume;
    }

    public void FadeOut(string soundName, float durationSec){
        int idx = Find(soundName);
        if(idx > -1) DOTween.To(() => ses[idx].Volume, v => ses[idx].Volume = v, 0, durationSec);
    }

    public void FadeoutAll(float durationSec){
        ses.ForEach(
            se => DOTween.To(() => se.Volume, v => se.Volume = v, 0, durationSec),
            where: se => se.RequireComponent
        );
    }

    public void VolumeTo(string soundName, float endValue, float durationSec){
        int idx = Find(soundName);
        if(idx > -1) DOTween.To(() => ses[idx].Volume, v => ses[idx].Volume = v, endValue, durationSec);
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


    [field: SerializeField, RenameField(nameof(Loop))]
    public bool Loop{ get; private set; }

    public bool IsPlaying => source.isPlaying;

    public float timeFromStart{
        get => RequireComponent ? source.time : -1;
        set{
            if(RequireComponent) source.time = value;
        }
    }
    

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
