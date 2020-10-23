using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;

public class JerryController : EnemyController
{
    [SerializeField] bool  isGoingUp  = true;
    [SerializeField] float periodSecs = 1.3f;

    [Space(10)]
    [SerializeField] Transform positionU;
    [SerializeField] Transform positionD;

    Tween currentTween;
    JellyView view;

    public override void OnSpawned()
    {
        rBody = transform.Find("Kasa").GetComponent<Rigidbody2D>();

        view = GetComponent<JellyView>();
        view.Init(isGoingUp);

        float posU = positionU.position.y;
        float posD = positionD.position.y;
        float diameter = posU - posD;

        currentTween = rBody
                       .DOMoveY(isGoingUp ? posU : posD, periodSecs)
                       .SetEase(Ease.InOutSine);
        
        float currentTimeNormalized = Mathf.InverseLerp
        (
            isGoingUp ? posD : posU,
            isGoingUp ? posU : posD,
            rBody.position.y
        );
        float currentTime = (1 - Mathf.Acos(currentTimeNormalized)) * periodSecs;
        currentTween.Goto(currentTime, andPlay: true);

        currentTween.OnComplete(() =>
        {
            Turn();

            currentTween = rBody
                .DOMoveY(isGoingUp ? posU : posD, periodSecs)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => Turn())
                .SetLoops(-1, LoopType.Yoyo);
        });


        Pauser.Instance.OnPause.Subscribe(_ =>
        {
            currentTween.Pause();
        })
        .AddTo(this);
        Pauser.Instance.OnPauseEnd.Subscribe(_ =>
        {
            currentTween.TogglePause();
        })
        .AddTo(this);
    }

    void Turn()
    {
        isGoingUp = !isGoingUp;
        if(isGoingUp) view.OnTurnUp();
        else          view.OnTurnDown();
    }
}
