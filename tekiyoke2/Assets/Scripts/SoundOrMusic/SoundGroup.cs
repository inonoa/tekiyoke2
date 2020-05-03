using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class SoundGroup : MonoBehaviour
{
    [SerializeField] SoundEffect[] ses;

    void Start(){
        AudioSource source4SE = gameObject.AddComponent<AudioSource>();

        foreach(SoundEffect se in ses){
            if(se.CanLoopAndStop){
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
            if(ses[i].CanLoopAndStop){
                ses[i].Stop();
            }
        }
    }
}

[Serializable]
public class SoundEffect{

    [SerializeField] string _Name;
    public string Name => _Name;

    [SerializeField] AudioClip _Clip;
    public AudioClip Clip => _Clip;

    [SerializeField] [Range(0, 1)] float _Volume = 1;
    public float Volume => _Volume;

    [SerializeField] bool _CanLoopAndStop = false;
    public bool CanLoopAndStop => _CanLoopAndStop;


    [field: SerializeField] [field: RenameField("Loop")]
    public bool Loop{ get; private set; }
    

    AudioSource source;

    public void Initialize(AudioSource source){
        this.source = source;
        if(CanLoopAndStop){
            source.volume = Volume;
            source.clip = Clip;
            source.loop = Loop;
        }
    }

    public void Play(){
        if(CanLoopAndStop){
            source.Play();
        }else{
            source.PlayOneShot(Clip, Volume);
        }
    }

    public void Stop(){
        if(CanLoopAndStop){
            source.Stop();
        }
        else Debug.LogError("止まらない！！");
    }
}
