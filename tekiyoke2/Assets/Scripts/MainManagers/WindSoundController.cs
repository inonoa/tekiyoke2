using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSoundController : MonoBehaviour
{
    SoundGroup soundGroup;

    readonly IReadOnlyList<string> windNames = new string[]{"Wind0", "Wind1", "Wind2"};

    void Start()
    {
        soundGroup = GetComponent<SoundGroup>();
        HeroDefiner.currentHero.HpCntr.hpChanged += (s, e) => OnHPChanged((s as HpCntr).HP);
        StartCoroutine(PlaySounds());
    }

    IEnumerator PlaySounds(){
        yield return new WaitForEndOfFrame();
        foreach(string windName in windNames){
            soundGroup.Play(windName);
        }
    }

    void OnHPChanged(int currentHP){
        foreach(string windName in windNames){
            soundGroup.VolumeTo(
                windName,
                new float[]{0, 1, 0.5f, 0.25f}[currentHP],
                0.5f
            );
        }
    }
}
