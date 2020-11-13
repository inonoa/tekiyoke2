using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;

public class JerryController : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [SerializeField] bool  isGoingUp  = true;
    [SerializeField] float periodSecs = 1.3f;

    [Space(10)]
    [SerializeField] Transform positionU;
    [SerializeField] Transform positionD;
    [SerializeField] Rigidbody2D RigidBody;

    [field: SerializeField, LabelText("DPCD")]
    public DPinEnemy DPCD{ get; private set; }

    JellyView view;

    void OnSpawned()
    {
        RigidBody = transform.Find("Kasa").GetComponent<Rigidbody2D>();

        view = GetComponent<JellyView>();
        view.Init(isGoingUp);

        float posU = positionU.position.y;
        float posD = positionD.position.y;
        float diameter = posU - posD;

        Tween firstTween = RigidBody.DOMoveY(isGoingUp ? posU : posD, periodSecs)
                                .SetEase(Ease.InOutSine);
        firstTween.GetPausable().AddTo(this);
        
        float currentTimeNormalized = Mathf.InverseLerp
        (
            isGoingUp ? posD : posU,
            isGoingUp ? posU : posD,
            RigidBody.position.y
        );
        float currentTime = (1 - Mathf.Acos(currentTimeNormalized)) * periodSecs;
        firstTween.Goto(currentTime, andPlay: true);

        firstTween.OnComplete(() =>
        {
            Turn();

            Tween mainTween = RigidBody
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


    public void Spawn()
    {
        gameObject.SetActive(true);
        OnSpawned();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
