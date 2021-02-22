using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FrontFogMover : MonoBehaviour
{
    [SerializeField] float selfSpeed = 0.1f;
    [SerializeField] float speedRateFromCamera = 0.03f;
    [SerializeField] float fadeInDuration = 3f;
    [SerializeField] float fadeOutDuration = 3f;
    [SerializeField] float fadeInOutGradationWidth = 2;
    
    [SerializeField] Image image;
    Material material;
    
    static readonly int OffsetX = Shader.PropertyToID("_OffsetX");
    static readonly int Alpha0UVX = Shader.PropertyToID("_Alpha0uvX");
    static readonly int Alpha1UVX = Shader.PropertyToID("_Alpha1uvX");

    void Awake()
    {
        material = image.material;
        material.SetFloat(Alpha0UVX, 1);
        material.SetFloat(Alpha1UVX, 1 + fadeInOutGradationWidth);
    }

    void Start()
    {
        lastCameraX = CameraController.CurrentCameraPos.x;
    }

    float lastCameraX;
    void Update()
    {
        float currentOffset = material.GetFloat(OffsetX);

        float selfDelta = selfSpeed * TimeManager.Current.DeltaTimeExceptHero;
        float cameraDelta = (CameraController.CurrentCameraPos.x - lastCameraX) * speedRateFromCamera;
        float deltaNormalized = (selfDelta + cameraDelta) / image.rectTransform.rect.width;
        
        material.SetFloat(OffsetX, currentOffset + deltaNormalized);
        
        lastCameraX = CameraController.CurrentCameraPos.x;
    }
    
    Tween currentFade;

    [Button]
    public void FadeIn(LR direction)
    {
        currentFade?.Kill();

        switch (direction)
        {
            case LR.L:
                if (material.GetFloat(Alpha0UVX) > material.GetFloat(Alpha1UVX))
                {
                    material.SetFloat(Alpha0UVX, 1);
                    material.SetFloat(Alpha1UVX, 1 + fadeInOutGradationWidth);
                }
                currentFade = DOTween.Sequence()
                    .Append(material.To(Alpha0UVX, -fadeInOutGradationWidth, fadeInDuration))
                    .Join(material.To(Alpha1UVX, 0, fadeInDuration));
                break;
            
            case LR.R:
                if (material.GetFloat(Alpha0UVX) < material.GetFloat(Alpha1UVX))
                {
                    material.SetFloat(Alpha0UVX, 0);
                    material.SetFloat(Alpha1UVX, - fadeInOutGradationWidth);
                }
                currentFade = DOTween.Sequence()
                    .Append(material.To(Alpha0UVX, 1 + fadeInOutGradationWidth, fadeInDuration))
                    .Join(material.To(Alpha1UVX, 1, fadeInDuration));
                break;
        }
    }

    [Button]
    public void FadeOut(LR direction)
    {
        currentFade?.Kill();
        
        switch (direction)
        {
            case LR.L:
                if (material.GetFloat(Alpha0UVX) < material.GetFloat(Alpha1UVX))
                {
                    material.SetFloat(Alpha0UVX, 1 + fadeInOutGradationWidth);
                    material.SetFloat(Alpha1UVX, 1);
                }
                currentFade = DOTween.Sequence()
                    .Append(material.To(Alpha0UVX, 0, fadeOutDuration))
                    .Join(material.To(Alpha1UVX, - fadeInOutGradationWidth, fadeOutDuration));
                break;
            
            case LR.R:
                if (material.GetFloat(Alpha0UVX) > material.GetFloat(Alpha1UVX))
                {
                    material.SetFloat(Alpha0UVX, - fadeInOutGradationWidth);
                    material.SetFloat(Alpha1UVX, 0);
                }
                currentFade = DOTween.Sequence()
                    .Append(material.To(Alpha0UVX, 1, fadeOutDuration))
                    .Join(material.To(Alpha1UVX, 1 + fadeInOutGradationWidth, fadeOutDuration));
                break;
        }
    }
}