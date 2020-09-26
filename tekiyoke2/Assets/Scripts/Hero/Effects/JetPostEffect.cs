using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JetPostEffect : MonoBehaviour
{
    PostEffectWrapper vignette;
    Tween vignetteTween;
    PostEffectWrapper blurY;
    Tween blurYTween;
    PostEffectWrapper blurT;
    Tween blurTTween;

    void Start()
    {
        vignette = CameraController.CurrentCamera.AfterEffects.Find("Vignette");
        blurY    = CameraController.CurrentCamera.AfterEffects.Find("BlurEdge1");
        blurT    = CameraController.CurrentCamera.AfterEffects.Find("BlurEdge2");
    }

    public void Ready()
    {
        vignette.IsActive = true;
        vignette.SetVolume(0);
        vignetteTween?.Kill();
        vignetteTween = DOTween.To(vignette.GetVolume, vignette.SetVolume, 2, 0.6f);

        blurY.IsActive = true;
        blurY.SetVolume(0);
        blurYTween?.Kill();
        blurYTween = DOTween.To(blurY.GetVolume, blurY.SetVolume, 2, 0.6f);

        blurT.IsActive = true;
        blurT.SetVolume(0);
        blurTTween?.Kill();
        blurTTween = DOTween.To(blurT.GetVolume, blurT.SetVolume, 2, 0.6f);
    }

    public void OnJet()
    {
        vignetteTween.Kill();
        Sequence endSeq_tmp = DOTween.Sequence();
        endSeq_tmp.Append(DOTween.To(vignette.GetVolume, vignette.SetVolume, -0.3f - vignette.GetVolume() / 2, 0.2f).SetEase(Ease.OutSine));
        endSeq_tmp.Append(DOTween.To(vignette.GetVolume, vignette.SetVolume, 0, 0.3f).SetEase(Ease.InOutSine));
        endSeq_tmp.onComplete += () => vignette.IsActive = false;
        vignetteTween = endSeq_tmp;

        blurYTween.Kill();
        blurYTween = DOTween.To(blurY.GetVolume, blurY.SetVolume, 0, 0.1f);
        blurYTween.onComplete += () => blurY.IsActive = false;

        blurTTween.Kill();
        blurTTween = DOTween.To(blurT.GetVolume, blurT.SetVolume, 0, 0.1f);
        blurTTween.onComplete += () => blurT.IsActive = false;
    }

    public void Exit()
    {
        new[]{vignette, blurY, blurT}.ForEach(pe => pe.IsActive = false);
    }
}
