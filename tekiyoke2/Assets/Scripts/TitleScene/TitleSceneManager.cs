using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class TitleSceneManager : SerializedMonoBehaviour
{
    [SerializeField] TitleLogo titleLogo;
    [SerializeField] CloudSpawner cloudSpawner;
    [SerializeField] AnyKeyToStart anyKeyToStart;
    [SerializeField] SoundGroup sounds;
    [SerializeField] IAskedInput input;
    [SerializeField] SaveDataManager saveDataManager;
    [SerializeField] float fadeOutToTransition = 1f;

    enum State{ FadingIn, Active, FadingOut }
    State state = State.FadingIn;

    void Start()
    {
        anyKeyToStart.FadeInCompleted.Subscribe(_ => state = State.Active);
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
                    SceneTransition.Start2ChangeScene("StageChoiceScene", SceneTransition.TransitionType.Default);
                });
            }
            else
            {
                SceneTransition.Start2ChangeScene("Tutorial", SceneTransition.TransitionType.WhiteOut);
            }
        }
    }
}
