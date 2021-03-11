using System;
using Config;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : SerializedMonoBehaviour
{
    [SerializeField] TitleLogo titleLogo;
    [SerializeField] CloudSpawner cloudSpawner;
    [SerializeField] AnyKeyToStart anyKeyToStart;
    [SerializeField] SoundGroup sounds;
    [SerializeField] IInput input;
    [SerializeField] SaveDataManager saveDataManager;
    [SerializeField] float fadeOutToTransition = 1f;
    [SerializeField] ISoundVolumeChanger soundVolumeChanger;

    [SerializeField] GameObject mainObj;
    [SerializeField] InitNameManager initName;

    enum State{ FadingIn, Active, FadingOut }
    State state = State.FadingIn;

    void Start()
    {
        anyKeyToStart.FadeInCompleted.Subscribe(_ => state = State.Active);
        
        saveDataManager.Init(() =>
        {
            if (!saveDataManager.PlayerNameInitiallized)
            {
                initName.Enter();
                initName.NameEntered.First().Subscribe(name =>
                {
                    saveDataManager.ChangePlayerName(name);
                    initName.Exit().Subscribe(_ =>
                    {
                        mainObj.SetActive(true);
                    });
                });
            }
            else
            {
                mainObj.SetActive(true);
            }
            soundVolumeChanger.ChangeSEVolume(saveDataManager.SEVolume);
            soundVolumeChanger.ChangeBGMVolume(saveDataManager.BGMVolume);
        });
    }

    void Update()
    {
        if(state != State.Active) return;

        if (input.AnyButtonDown())
        {
            state = State.FadingOut;
            
            if (saveDataManager.TutorialFinished)
            {
                titleLogo.FadeOut();
                cloudSpawner.FadeOut();
                anyKeyToStart.Disappear();
                sounds.Play("Push");
                
                DOVirtual.DelayedCall(fadeOutToTransition, () =>
                {
                    SceneTransition.StartToChangeScene<DefaultTransitionView>("StageChoiceScene");
                });
            }
            else
            {
                SceneTransition.StartToChangeScene<WhiteOutTransitionView>("Tutorial");
            }
        }
    }
}
