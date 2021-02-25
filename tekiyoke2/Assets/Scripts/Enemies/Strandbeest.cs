using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class Strandbeest : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [SerializeField] Vector2 oneStepCorrection;
    
    [SerializeField] Transform body;
    [SerializeField] Transform rightLeg;
    [SerializeField] Transform rightLegTip;
    [SerializeField] Transform leftLeg;
    [SerializeField] Transform leftLegTip;
    [SerializeField] Transform windmill;
    [SerializeField] Transform core;

    [field: SerializeField, LabelText(nameof(DPCD))]
    public DPinEnemy DPCD { get; private set; }

    Tween moveTween;

    [SerializeField] float windmillRotateSpeed = 500;
    [SerializeField, ReadOnly] float timeScale = 1;

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        Step();
        
        DOTween.To
        (
            () => timeScale,
            ts =>
            {
                timeScale = ts;
                moveTween.timeScale = ts;
            },
            0.4f,
            3.7f
        )
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
    }

    void Step()
    {
        float dur = 0.5f;
        Ease ease = Ease.InOutSine;
        
        moveTween = DOTween.Sequence()
            .Append(body.DORotateAroundRelative(() => leftLegTip.position, 45, dur * 2).SetEase(ease))
            .Join(rightLeg.DOLocalRotate(new Vector3(0, 0, 45f), dur).SetRelative().SetEase(ease))
            .Append(body.DORotateAroundRelative(() => leftLegTip.position, -67.5f, dur * 1.5f).SetEase(ease))
            .Append(body.DORotateAroundRelative(() => rightLegTip.position, -45f, dur).SetEase(ease))
            .Join(leftLeg.DOLocalRotate(new Vector3(0, 0, 45f), dur).SetRelative().SetEase(ease))
            .Append(body.DORotateAroundRelative(() => rightLegTip.position, 22.5f, dur).SetEase(ease))
            .Join(core.DOLocalRotate(new Vector3(0, 0, 45f), dur).SetRelative().SetEase(ease))
            .OnComplete(() => Step())
            ;
    }

    void Update()
    {
        windmill.Rotate(new Vector3(0, 0, windmillRotateSpeed * timeScale * timeScale * TimeManager.Current.DeltaTimeExceptHero));
    }
}
