using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TitleLogo : MonoBehaviour
{
    [SerializeField] Image logoImage;
    [SerializeField] SoundGroup sounds;
    [Space(10)] [SerializeField] float soundDelay = 0.1f;
    [SerializeField] float fadeInDuration = 1f;
    [SerializeField] float fadeInDelay = 0.5f;
    [Space(10)]
    [SerializeField] float fadeOutDuration = 1;
    [SerializeField] float fadeOutMoveDistance = 300f;
    [SerializeField] float fadeOutMoveDuration = 1f;

    void Start()
    {
        InitShaderProperties();
        
        Material titleMat = logoImage.material;

        DOVirtual.DelayedCall(soundDelay, () => sounds.Play("TitleIn"));

        Sequence fadeIn = DOTween.Sequence();

        fadeIn.AppendInterval(fadeInDelay)
            .Append(titleMat.To("_DissolveThreshold0", -0.2f, fadeInDuration))
            .Join(titleMat.To("_DissolveThreshold1", 0, fadeInDuration))
            .Join(titleMat.To("_GradationThreshold0", -0.4f, fadeInDuration))
            .Join(titleMat.To("_GradationThreshold1", 0, fadeInDuration));

        fadeIn.AppendInterval(0.35f)
            .Append(titleMat.To("_Black2SpriteCol", 1, 1).SetEase(Ease.OutQuint));
    }

    void InitShaderProperties()
    {
        Material titleMat = logoImage.material;
        
        titleMat.SetFloat("_DissolveThreshold0", 1);
        titleMat.SetFloat("_DissolveThreshold1", 1.2f);
        titleMat.SetFloat("_GradationThreshold0", 1);
        titleMat.SetFloat("_GradationThreshold1", 1.4f);
        titleMat.SetFloat("_Black2SpriteCol", 0);
    }
    
    public void FadeOut()
    {
        logoImage.transform.DOLocalMoveX(-fadeOutMoveDistance, fadeOutMoveDuration).SetRelative();
        logoImage.DOFade(0, fadeOutDuration);
    }
}