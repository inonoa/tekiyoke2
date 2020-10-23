using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;
using DG.Tweening;
using Sirenix.OdinInspector;

public class JerryController : EnemyController
{

    float amplitude;
    float centerPositionY;

    [SerializeField] bool isGoingUp = true;

    static readonly float speedYEpsilon = 0.01f;
    static readonly float linear2Sin = 80;

    [SerializeField] int lightSeconds = 10;
    [SerializeField] int unlightFrames = 30;

    [SerializeField] SpriteRenderer kasaSR;
    [SerializeField] SpriteRenderer asiSR;
    [SerializeField] Sprite kasaSpriteUp;
    [SerializeField] Sprite asiSpriteUp;
    [SerializeField] Sprite kasaSpriteDown;
    [SerializeField] Sprite asiSpriteDown;
    [SerializeField] SpriteRenderer lightSR;
    [SerializeField] SoundGroup soundGroup;

    //定数シュッと置いとく方法を探している
    static readonly (string _Volume, string Kaze) c = ("_Volume", "Kaze");

    [SerializeField] [ReadOnly] float posU;
    [SerializeField] [ReadOnly] float posD;

    Tween moveTweeen;

    public override void OnSpawned()
    {
        rBody = transform.Find("Kasa").GetComponent<Rigidbody2D>();

        posU = transform.Find("PositionU").position.y;
        posD = transform.Find("PositionD").position.y;
        amplitude = (posU - posD) / 2;

        kasaSR.sprite = isGoingUp ? kasaSpriteUp : kasaSpriteDown;
        asiSR.sprite  = isGoingUp ? asiSpriteUp  : asiSpriteDown;
        lightSR.material.SetFloat(c._Volume, isGoingUp ? 1 : 0);

        Tween firstTween = rBody
            .DOMoveY(isGoingUp ? posU : posD, 1.6f)
            .SetEase(Ease.InOutSine) //goto的なの挟みたい
            .OnComplete(() =>
            {
                DOTween.Sequence()
                .Append
                (
                    rBody
                        .DOMoveY(isGoingUp ? - amplitude * 2 : amplitude * 2, 1.6f)
                        .SetRelative()
                        .SetEase(Ease.InOutSine)
                )
                .AppendCallback(() =>
                {
                    rBody.MovePosition(new Vector2
                    (
                        rBody.transform.position.x,
                        isGoingUp ? posD : posU
                    ));
                })
                .Append
                (
                    rBody
                        .DOMoveY(isGoingUp ? amplitude * 2 : - amplitude * 2, 1.6f)
                        .SetRelative()
                        .SetEase(Ease.InOutSine)
                )
                .AppendCallback(() =>
                {
                    rBody.MovePosition(new Vector2
                    (
                        rBody.transform.position.x,
                        isGoingUp ? posU : posD
                    ));
                })
                .SetLoops(-1);
            });
    }

    new void Update()
    {
        
        // if(JellyPosY <= centerPositionY - amplitude + 1)
        // {
        //     IsGoingUp = true;
        //     kasaSR.sprite = kasaSpriteUp;
        //     asiSR.sprite  = asiSpriteUp;
        //     StartCoroutine(Light());
        //     if(MyMath.DistanceXY(transform.position, HeroDefiner.CurrentHeroPos) < 625)
        //     {
        //         DOVirtual.DelayedCall(UnityEngine.Random.Range(0f, 0.5f), () =>
        //             soundGroup.Play(c.Kaze));
        //     }
        // }
    }

    IEnumerator Light(){

        int lightFrames = 10;

        for(int i=0;i<lightFrames;i++){
            lightSR.material.SetFloat(c._Volume, (i+1)/(float)lightFrames);
            yield return null;
        }
    }
    IEnumerator Unlight(){

        for(int i=0;i<unlightFrames;i++){
            lightSR.material.SetFloat(c._Volume, (unlightFrames-1-i)/(float)unlightFrames);
            yield return null;
        }
    }
}
