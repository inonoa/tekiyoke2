using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Tsuchihokori : MonoBehaviour, IReusable
{
    [SerializeField] SpriteRenderer tsuchi;
    [SerializeField] Sprite[] tsuchiSprites;
    [SerializeField] SpriteRenderer kemuri;
    [SerializeField] Sprite[] kemuriSprites;

    [SerializeField] Vector3 positionFromHero;

    List<Tween> currentTweens = new List<Tween>();

    List<Tween> PlayAnim()
    {
        List<Tween> tweens = new List<Tween>();

        float spriteChangeSec = 0.1f;

        tweens.Add(
            DOTween.Sequence()
            .Append(DOVirtual.DelayedCall(spriteChangeSec, () => 
            {
                tsuchi.sprite = tsuchiSprites[1];
                kemuri.sprite = kemuriSprites[1];
            },
            ignoreTimeScale: false)
            .AsHeros()
            )
            .Append(DOVirtual.DelayedCall(spriteChangeSec, () =>
            {
                tsuchi.sprite = tsuchiSprites[2];
                kemuri.sprite = kemuriSprites[2];
            },
            ignoreTimeScale: false)
            .AsHeros()
            )
            .Append(DOVirtual.DelayedCall(spriteChangeSec, () =>
            {
                tsuchi.sprite = tsuchiSprites[3];
                kemuri.sprite = kemuriSprites[3];
                tweens.Add(
                    kemuri.transform.DOLocalMoveX(-50, 0.7f)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        InUse = false;
                    })
                    .AsHeros()
                );
            }, 
            ignoreTimeScale: false)
            .AsHeros()
            )
            .Append(DOVirtual.DelayedCall(0.1f, () =>
            {
                tweens.Add(kemuri.DOFade(0, 0.6f).AsHeros());
            },
            ignoreTimeScale: false)
            .AsHeros()
            )
            .AsHeros()
        );

        return tweens;
    }

    public void Activate(string heroDirStr){

        InUse = true;

        Init(heroDirStr == "r");

        currentTweens.ForEach(tw => tw.Kill());
        currentTweens = PlayAnim();
    }

    void Init(bool right)
    {
        transform.position = HeroDefiner.CurrentPos + new Vector3(
            right ? positionFromHero.x : - positionFromHero.x,
            positionFromHero.y,
            positionFromHero.z
        );
        transform.localScale = new Vector3(right ? 1 : -1, 1, 1);

        tsuchi.sprite = tsuchiSprites[0];

        kemuri.sprite = kemuriSprites[0];
        kemuri.color = new Color(1,1,1, 0.5f);
        kemuri.transform.localPosition = new Vector3(0,0,0);
    }

    public bool InUse{ get; private set; } = false;
}
