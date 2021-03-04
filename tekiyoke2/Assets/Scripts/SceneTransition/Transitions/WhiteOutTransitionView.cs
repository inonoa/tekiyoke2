
using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class WhiteOutTransitionView : ISceneTransitionView
{
    [SerializeField] Image whiteOutImage;
    
    public IObservable<Unit> OnTransitionStart(SceneTransition sceneTransition)
    {
        const float duration = 5f;
                
        PostEffectWrapper noise_ = CameraController.Current?.AfterEffects?.Find("Noise");
        if(noise_ != null) DOTween.To(noise_.GetVolume, noise_.SetVolume, 0, duration / 2);
        
        var onEnd = new Subject<Unit>();

        Material whiteOutMat = whiteOutImage.material;
        whiteOutMat.SetFloat("_Alpha", 0.3f);
        whiteOutMat.SetFloat("_Whiteness", 0.05f);
        whiteOutMat.To("_Alpha", 1, duration / 1.5f);
        whiteOutMat.To("_Whiteness", 0.9f, duration).SetEase(Ease.InOutCubic)
            .onComplete += () => onEnd.OnNext(Unit.Default);

        return onEnd;
    }

    public void OnNextSceneStart(SceneTransition sceneTransition)
    {
        float duration = 3;
        
        Material whiteOutMat = whiteOutImage.material;
        whiteOutMat.SetFloat("_Alpha", 1f);
        whiteOutMat.SetFloat("_Whiteness", 1);
        whiteOutMat.To("_Alpha", 0, duration);
        whiteOutMat.To("_Whiteness", 0.05f, duration).SetEase(Ease.OutCubic);
        
        PostEffectWrapper noise = CameraController.Current?.AfterEffects?.Find("Noise");
        if(noise is null) return;
        
        noise.SetVolume(0);
        DOTween.To(noise.GetVolume, noise.SetVolume, 1, 1);
    }
}