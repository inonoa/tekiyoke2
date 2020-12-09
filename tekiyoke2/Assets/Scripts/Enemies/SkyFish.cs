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
    [SerializeField] SoundGroup sounds;

    float soundVolumeMax;

    void Start()
    {
        spriteRenderer.sprite = eyeToRight ? spriteRight : spriteLeft;
        DOTweenPath.tween.GetPausable();
        DOTweenPath.tween.onStepComplete += () =>
        {
            eyeToRight = !eyeToRight;
            spriteRenderer.sprite = eyeToRight ? spriteRight : spriteLeft;
        };
        soundVolumeMax = sounds.GetVolume("fly");
    }

    void Update()
    {
        float distFromHero = MyMath.DistanceXY(HeroDefiner.CurrentPos, this.transform.position);
        sounds.SetVolume
        (
            "fly",
            soundVolumeMax * Mathf.Pow(0.5f, distFromHero / 200)
        );
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        DOTweenPath.tween.TogglePause();
        sounds.Play("fly");
    }

    public void Hide()
    {
        DOTweenPath.tween.Pause();
        sounds.Stop("fly");
        gameObject.SetActive(false);
    }
}