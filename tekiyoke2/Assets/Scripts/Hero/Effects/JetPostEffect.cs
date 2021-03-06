﻿using System.Collections;
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
        vignette = CameraController.Current.AfterEffects.Find("Vignette");
        blurY    = CameraController.Current.AfterEffects.Find("BlurEdge1");
        blurT    = CameraController.Current.AfterEffects.Find("BlurEdge2");
    }

    public void Ready()
    {
        vignette.SetActive(true);
        vignette.SetVolume(0);
        vignetteTween?.Kill();
        vignetteTween = DOTween.To(vignette.GetVolume, vignette.SetVolume, 2, 0.6f).AsHeros();

        blurY.SetActive(true);
        blurY.SetVolume(0);
        blurYTween?.Kill();
        blurYTween = DOTween.To(blurY.GetVolume, blurY.SetVolume, 2, 0.6f).AsHeros();

        blurT.SetActive(true);
        blurT.SetVolume(0);
        blurTTween?.Kill();
        blurTTween = DOTween.To(blurT.GetVolume, blurT.SetVolume, 2, 0.6f).AsHeros();
    }

    public void OnJet()
    {
        vignetteTween.Kill();
        Sequence endSeq_tmp = DOTween.Sequence();
        endSeq_tmp.Append(DOTween.To(vignette.GetVolume, vignette.SetVolume, -0.3f - vignette.GetVolume() / 2, 0.2f).SetEase(Ease.OutSine));
        endSeq_tmp.Append(DOTween.To(vignette.GetVolume, vignette.SetVolume, 0, 0.3f).SetEase(Ease.InOutSine));
        endSeq_tmp.onComplete += () => vignette.SetActive(false);
        endSeq_tmp.AsHeros();
        vignetteTween = endSeq_tmp;

        blurYTween.Kill();
        blurYTween = DOTween.To(blurY.GetVolume, blurY.SetVolume, 0, 0.1f).AsHeros();
        blurYTween.onComplete += () => blurY.SetActive(false);

        blurTTween.Kill();
        blurTTween = DOTween.To(blurT.GetVolume, blurT.SetVolume, 0, 0.1f).AsHeros();
        blurTTween.onComplete += () => blurT.SetActive(false);
    }

    public void Exit()
    {
        new[]{vignette, blurY, blurT}.ForEach(pe => pe.SetActive(false));
    }
}
