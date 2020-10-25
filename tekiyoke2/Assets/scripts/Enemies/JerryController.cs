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

    JellyView view;

    public override void OnSpawned()
    {
        rBody = transform.Find("Kasa").GetComponent<Rigidbody2D>();

        view = GetComponent<JellyView>();
        view.Init(isGoingUp);

        float posU = positionU.position.y;
        float posD = positionD.position.y;
        float diameter = posU - posD;

        Tween firstTween = rBody.DOMoveY(isGoingUp ? posU : posD, periodSecs)
                                .SetEase(Ease.InOutSine);
        firstTween.GetPausable().AddTo(this);
        
        float currentTimeNormalized = Mathf.InverseLerp
        (
            isGoingUp ? posD : posU,
            isGoingUp ? posU : posD,
            rBody.position.y
        );
        float currentTime = (1 - Mathf.Acos(currentTimeNormalized)) * periodSecs;
        firstTween.Goto(currentTime, andPlay: true);

        firstTween.OnComplete(() =>
        {
            Turn();

            Tween mainTween = rBody
                .DOMoveY(isGoingUp ? posU : posD, periodSecs)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .OnStepComplete(() => Turn());
                
            mainTween.GetPausable().AddTo(this);
        });
    }

    void Turn()
    {
        isGoingUp = !isGoingUp;
        if(isGoingUp) view.OnTurnUp();
        else          view.OnTurnDown();
    }
}
