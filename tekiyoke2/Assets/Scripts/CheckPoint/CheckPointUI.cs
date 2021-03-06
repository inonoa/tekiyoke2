﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class CheckPointUI : MonoBehaviour
{
    [SerializeField] float fadeInSecs    = 0.7f;
    [SerializeField] float intervalSecs  = 1f;
    [SerializeField] float fadeOutSecs   = 0.7f;
    [SerializeField] float slideDistance = 100;

    [SerializeField] CheckPointsManager checkPointsManager;
    [SerializeField] Text text;
    [SerializeField] CanvasGroup canvasGroup;

    
    void Start()
    {
        checkPointsManager.PassedNewCheckPoint.Subscribe(checkPoint =>
        {
            canvasGroup.DOFade(0, 0);
            canvasGroup.transform.DOLocalMoveX(slideDistance, 0);

            text.text = $"checkpoint: {checkPoint.Name} passed";

            Tween fadeInOut = DOTween.Sequence()
                .Append
                (
                    canvasGroup.transform
                        .DOLocalMoveX(0, fadeInSecs)
                        .SetEase(Ease.OutQuint)
                )
                .Join
                (
                    canvasGroup
                        .DOFade(1, fadeInSecs)
                )
                .AppendInterval(intervalSecs)
                .Append
                (
                    canvasGroup.transform
                        .DOLocalMoveX(-slideDistance, fadeOutSecs)
                        .SetRelative()
                        .SetEase(Ease.InOutSine)
                )
                .Join
                (
                    canvasGroup
                        .DOFade(0, fadeOutSecs)
                )
                .SetUpdate(true);

            fadeInOut.GetPausable().AddTo(this);
        });
    }
}
