
using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[Serializable]
public class WindAndBlurTransitionView : ISceneTransitionView
{
    [SerializeField] WindAndBlur windAndBlur = null;
    [SerializeField] Image scshoImage = null;
    [SerializeField] Transform bgTransformForScSho;

    static Texture2D scSho;
    
    public IObservable<Unit> OnTransitionStart(SceneTransition sceneTransition)
    {
        PostEffectWrapper noise = CameraController.Current?.AfterEffects?.Find("Noise");
        if(noise != null) DOTween.To(noise.GetVolume, noise.SetVolume, 0, 2f);

        var end = new Subject<Unit>();
        
        DOVirtual.DelayedCall(1.2f, () =>
        {
            var windblur = Object.Instantiate(windAndBlur, sceneTransition.transform.parent);
            windblur.OnEnd.Subscribe(ss =>
            {
                scSho = ss;
                end.OnNext(Unit.Default);
            });
            windblur.transform.SetAsLastSibling();
        });

        return end;
    }

    public void OnNextSceneStart(SceneTransition sceneTransition)
    {
        Image scshoImg = Object.Instantiate(scshoImage, bgTransformForScSho);
        scshoImg.sprite = Sprite.Create(scSho, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f,0.5f));

        scSho = null;
    }
}