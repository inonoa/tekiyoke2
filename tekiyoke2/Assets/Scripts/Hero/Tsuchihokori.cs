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

    public Vector3 positionFromHero;
    public void Activate(){

        InUse = true;
        transform.position = HeroDefiner.CurrentHeroPos + positionFromHero;
        tsuchi.sprite = tsuchiSprites[0];
        kemuri.sprite = kemuriSprites[0];
        kemuri.color = new Color(1,1,1, 0.5f);
        kemuri.transform.localPosition = new Vector3(0,0,0);

        //つちのアニメ

        DOVirtual.DelayedCall(0.1f, () => tsuchi.sprite = tsuchiSprites[1] );
        DOVirtual.DelayedCall(0.2f, () => tsuchi.sprite = tsuchiSprites[2] );
        DOVirtual.DelayedCall(0.3f, () => tsuchi.sprite = tsuchiSprites[3] );

        //けむりのアニメ

        DOVirtual.DelayedCall(0.1f, () => {
            kemuri.sprite = kemuriSprites[1];
        });
        DOVirtual.DelayedCall(0.2f, () => {
            kemuri.sprite = kemuriSprites[2];
        });

        DOVirtual.DelayedCall(0.3f, () => {
            kemuri.sprite = kemuriSprites[3];
            kemuri.transform.DOLocalMoveX(-50, 0.7f);
        });

        DOVirtual.DelayedCall(0.4f, () => {
            kemuri.DOFade(0, 0.6f);
        });

        //おわり

        DOVirtual.DelayedCall(1f, () => {
            gameObject.SetActive(false);
            InUse = false;
        });
        
    }

    public bool InUse{ get; private set; }
}
