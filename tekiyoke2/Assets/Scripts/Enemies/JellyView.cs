using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class JellyView : MonoBehaviour
{
    [SerializeField] float lightSeconds      = 0.2f;
    [SerializeField] float unlightSeconds    = 0.5f;
    [SerializeField] float nearHeroThreshold = 625;

    [Space(10)]
    [SerializeField] SpriteRenderer kasaSR;
    [SerializeField] SpriteRenderer asiSR;
    [SerializeField] SpriteRenderer lightSR;
    [SerializeField] Sprite kasaSpriteUp;
    [SerializeField] Sprite asiSpriteUp;
    [SerializeField] Sprite kasaSpriteDown;
    [SerializeField] Sprite asiSpriteDown;
    [SerializeField] SoundGroup soundGroup;
    

    Material lightMaterial;

    Tween currentTween;


    public void Init(bool isGoingUp)
    {
        lightMaterial = lightSR.material;

        kasaSR.sprite = isGoingUp ? kasaSpriteUp : kasaSpriteDown;
        asiSR.sprite  = isGoingUp ? asiSpriteUp  : asiSpriteDown;
        lightMaterial.SetFloat("_Volume", isGoingUp ? 1 : 0);
    }

    public void OnTurnUp()
    {
        kasaSR.sprite = kasaSpriteUp;
        asiSR.sprite  = asiSpriteUp;

        currentTween?.Kill();
        currentTween = DOTween.To
        (
            () => lightMaterial.GetFloat("_Volume"),
            v  => lightMaterial.SetFloat("_Volume", v),
            1,
            lightSeconds
        )
        .FollowTimeScale(aroundHero: false);
        currentTween.GetPausable().AddTo(this);

        if(MyMath.DistanceXY(transform.position, HeroDefiner.CurrentHeroPos) < nearHeroThreshold)
        {
            DOVirtual.DelayedCall(UnityEngine.Random.Range(0, 0.5f), () =>
            {
                soundGroup.Play("Kaze");
            });
        }
    }

    public void OnTurnDown()
    {
        kasaSR.sprite = kasaSpriteDown;
        asiSR.sprite  = asiSpriteDown;

        currentTween?.Kill();
        currentTween = DOTween.To
        (
            () => lightMaterial.GetFloat("_Volume"),
            v  => lightMaterial.SetFloat("_Volume", v),
            0,
            unlightSeconds
        )
        .FollowTimeScale(aroundHero: false);
        currentTween.GetPausable().AddTo(this);
    }
}
