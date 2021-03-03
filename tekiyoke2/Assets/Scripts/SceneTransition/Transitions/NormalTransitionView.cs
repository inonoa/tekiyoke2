using UnityEngine;
using System;
using DG.Tweening;
using UniRx;
using Object = UnityEngine.Object;

[Serializable]
public class NormalTransitionView : ISceneTransitionView
{
    [SerializeField] Curtain4SceneEndMover curtain4SceneEnd;
    [SerializeField] Curtain4SceneStartMover curtain4SceneStart;
    
    public IObservable<Unit> OnTransitionStart(SceneTransition sceneTransition)
    {
        PostEffectWrapper noise = CameraController.Current?.AfterEffects?.Find("Noise");
        if(noise != null) DOTween.To(noise.GetVolume, noise.SetVolume, 0, 1);
        
        var curtain = Object.Instantiate(curtain4SceneEnd, sceneTransition.transform);
        return curtain.OnMoveEnd;
    }

    public void OnNextSceneStart(SceneTransition sceneTransition)
    {
        Object.Instantiate(curtain4SceneStart, sceneTransition.transform);
        
        PostEffectWrapper noise = CameraController.Current?.AfterEffects?.Find("Noise");
        if(noise is null) return;
        
        noise.SetVolume(0);
        DOTween.To(noise.GetVolume, noise.SetVolume, 1, 1);
    }
}