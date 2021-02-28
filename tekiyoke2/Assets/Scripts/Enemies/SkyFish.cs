using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Sirenix.OdinInspector;
using UniRx;
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

    [SerializeField] int bufferFramesForAngle;

    void Start()
    {
        spriteRenderer.sprite = eyeToRight ? spriteRight : spriteLeft;
        DOTweenPath.tween.GetPausable();
        soundVolumeMax = sounds.GetVolume("fly");

        this.UpdateAsObservable()
            .Select(_ => transform.position)
            .Buffer(bufferFramesForAngle, 1)
            .Subscribe(positions =>
            {
                float angle = 180 + Vector2.SignedAngle(Vector2.right, (positions.Last() - positions.First()));
                transform.rotation = Quaternion.Euler(0, 0, angle);
            })
            .AddTo(this);
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