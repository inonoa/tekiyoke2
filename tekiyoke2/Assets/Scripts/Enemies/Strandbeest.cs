using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Strandbeest : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [SerializeField] Transform body;
    [SerializeField] Transform rightLeg;
    [SerializeField] Transform rightLegTip;
    [SerializeField] Collider2D rightWallSensor;
    [SerializeField] Transform leftLeg;
    [SerializeField] Transform leftLegTip;
    [SerializeField] Collider2D leftWallSensor;
    [SerializeField] Transform windmill;
    [SerializeField] Transform core;

    [field: SerializeField, LabelText(nameof(DPCD))]
    public DPinEnemy DPCD { get; private set; }

    Tween moveTween;
    [SerializeField] bool isMovingRight = true;

    [SerializeField] float windmillRotateSpeed = 500;
    [SerializeField, ReadOnly] float moveSpeedRate = 1;

    public void Spawn()
    {
        gameObject.SetActive(true);
        moveTween?.TogglePause();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        moveTween?.Pause();
    }

    void Start()
    {
        if(isMovingRight) StepRight();
        else StepLeft();

        rightWallSensor.OnTriggerEnter2DAsObservable()
            .Where(other => other.CompareTag(Tags.Terrain))
            .Where(_ => isMovingRight)
            .Subscribe(_ => Turn());
        
        leftWallSensor.OnTriggerEnter2DAsObservable()
            .Where(other => other.CompareTag(Tags.Terrain))
            .Where(_ => !isMovingRight)
            .Subscribe(_ => Turn());

        InitWind();
    }

    void InitWind()
    {
        DOTween.To
            (
                () => moveSpeedRate,
                ts =>
                {
                    moveSpeedRate = ts;
                    moveTween.timeScale = ts;
                },
                0.5f,
                6.7f
            )
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutBack)
            .GetPausable()
            .AddTo(this);
    }

    [SerializeField] float oneStepSeconds = 5;
    [SerializeField] Ease ease = Ease.InOutSine;

    void StepRight()
    {
        float sectionDur = oneStepSeconds / 5.5f;

        moveTween = DOTween.Sequence()
            .Append(body.DORotateAroundRelative(() => leftLegTip.position, 45, sectionDur * 2).SetEase(ease))
            .Join(rightLeg.DOLocalRotate(new Vector3(0, 0, 45f), sectionDur * 2).SetRelative().SetEase(ease))

            .Append(body.DORotateAroundRelative(() => leftLegTip.position, -67.5f, sectionDur * 1.5f).SetEase(ease))

            .Append(body.DORotateAroundRelative(() => rightLegTip.position, -45f, sectionDur).SetEase(ease))
            .Join(leftLeg.DOLocalRotate(new Vector3(0, 0, 45f), sectionDur).SetRelative().SetEase(ease))

            .Append(body.DORotateAroundRelative(() => rightLegTip.position, 22.5f, sectionDur).SetEase(ease))
            .Join(core.DOLocalRotate(new Vector3(0, 0, 45f), sectionDur).SetRelative().SetEase(ease));
            
        moveTween.OnComplete(StepRight);
        moveTween.GetPausable().AddTo(this);
    }

    void StepLeft()
    {
        float sectionDur = oneStepSeconds / 5.5f;

        moveTween = DOTween.Sequence()
            .Append(body.DORotateAroundRelative(() => rightLegTip.position, -45, sectionDur * 2).SetEase(ease))
            .Join(leftLeg.DOLocalRotate(new Vector3(0, 0, -45f), sectionDur * 2).SetRelative().SetEase(ease))

            .Append(body.DORotateAroundRelative(() => rightLegTip.position, 67.5f, sectionDur * 1.5f).SetEase(ease))

            .Append(body.DORotateAroundRelative(() => leftLegTip.position, 45f, sectionDur).SetEase(ease))
            .Join(rightLeg.DOLocalRotate(new Vector3(0, 0, -45f), sectionDur).SetRelative().SetEase(ease))

            .Append(body.DORotateAroundRelative(() => leftLegTip.position, -22.5f, sectionDur).SetEase(ease))
            .Join(core.DOLocalRotate(new Vector3(0, 0, -45f), sectionDur).SetRelative().SetEase(ease));
            
        moveTween.OnComplete(StepLeft);
        moveTween.GetPausable().AddTo(this);
    }

    void Update()
    {
        windmill.Rotate(new Vector3(0, 0, windmillRotateSpeed * moveSpeedRate * moveSpeedRate * TimeManager.Current.DeltaTimeExceptHero));
    }

    [Button]
    void Turn()
    {
        isMovingRight = !isMovingRight;

        moveTween.PlayBackwards();

        if (isMovingRight)
        {
            moveTween.OnRewind(StepRight);
        }
        else
        {
            moveTween.OnRewind(StepLeft);
        }
    }

    void OnDestroy()
    {
        moveTween?.Kill();
    }
}
