using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Sirenix.OdinInspector;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

public class SkyFish : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [SerializeField] bool eyeToRight = false;
    
    [field: Space(10)]
    [field: SerializeField, LabelText(nameof(DPCD))]
    public DPinEnemy DPCD { get; private set; }

    [FormerlySerializedAs("DOTweenPathL")] [SerializeField] DOTweenPath DOTweenPath;
    [SerializeField] float secondsToFirstTurn = 1;
    
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;
    [SerializeField] SoundGroup sounds;

    float soundVolumeMax;

    void Start()
    {
        spriteRenderer.sprite = eyeToRight ? spriteRight : spriteLeft;
        DOTweenPath.tween.GetPausable();
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
        this.StartPausableCoroutine(Turn());
    }

    IEnumerator Turn()
    {
        float lastposition = 0;
        while (true)
        {
            while (true)
            {
                float turn1 = 4.1f;
                if (lastposition < turn1 && DOTweenPath.tween.position > turn1)
                {
                    eyeToRight = !eyeToRight;
                    spriteRenderer.sprite = eyeToRight ? spriteRight : spriteLeft;
                    break;
                }

                lastposition = DOTweenPath.tween.position;
                yield return null;
            }
            while (true)
            {
                float turn2 = 0.1f;
                if (lastposition < turn2 && DOTweenPath.tween.position > turn2)
                {
                    eyeToRight = !eyeToRight;
                    spriteRenderer.sprite = eyeToRight ? spriteRight : spriteLeft;
                    break;
                }

                lastposition = DOTweenPath.tween.position;
                yield return null;
            }
        }
    }

    public void Hide()
    {
        DOTweenPath.tween.Pause();
        sounds.Stop("fly");
        gameObject.SetActive(false);
    }
}