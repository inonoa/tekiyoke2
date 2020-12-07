using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class SkyFish : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [SerializeField] bool eyeToRight = false;
    
    [field: Space(10)]
    [field: SerializeField, LabelText(nameof(DPCD))]
    public DPinEnemy DPCD { get; private set; }

    [SerializeField] DOTweenPath DOTweenPath;
    
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    void Start()
    {
        spriteRenderer.sprite = eyeToRight ? spriteRight : spriteLeft;
        DOTweenPath.tween.GetPausable();
        DOTweenPath.tween.onStepComplete += () =>
        {
            eyeToRight = !eyeToRight;
            spriteRenderer.sprite = eyeToRight ? spriteRight : spriteLeft;
        };
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        DOTweenPath.tween.TogglePause();
    }

    public void Hide()
    {
        DOTweenPath.tween.Pause();
        gameObject.SetActive(false);
    }
}