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
    }

    void Step()
    {
        float dur = 0.6f;
        
        var seq = DOTween.Sequence();
        seq
            .Append(body.DORotateAroundRelative(() => leftLegTip.position, 45, dur).SetEase(Ease.InOutSine))
            .Join(rightLeg.DOLocalRotate(new Vector3(0, 0, 45f), dur).SetRelative().SetEase(Ease.InOutSine))
            .Append(body.DORotateAroundRelative(() => leftLegTip.position, -67.5f, dur).SetEase(Ease.InOutSine))
            .Append(body.DORotateAroundRelative(() => rightLegTip.position, -45f, dur).SetEase(Ease.InOutSine))
            .Join(leftLeg.DOLocalRotate(new Vector3(0, 0, 45f), dur).SetRelative().SetEase(Ease.InOutSine))
            .Append(body.DORotateAroundRelative(() => rightLegTip.position, 22.5f, dur).SetEase(Ease.InOutSine))
            .Join(core.DOLocalRotate(new Vector3(0, 0, 45f), dur).SetRelative().SetEase(Ease.InOutSine))
            .OnComplete(() => Step())
            ;
    }

    void Update()
    {
        windmill.Rotate(new Vector3(0, 0, 200 * TimeManager.Current.DeltaTimeExceptHero));
    }
}
