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

    Tween[] tweens2restart = new Tween[8];
    Tween[] tweensNot2restart = new Tween[2];

    void Awake(){

        //つちのアニメ

        tweens2restart[0] = DOVirtual.DelayedCall(0.1f, () => tsuchi.sprite = tsuchiSprites[1] );
        tweens2restart[1] = DOVirtual.DelayedCall(0.2f, () => tsuchi.sprite = tsuchiSprites[2] );
        tweens2restart[2] = DOVirtual.DelayedCall(0.3f, () => tsuchi.sprite = tsuchiSprites[3] );

        //けむりのアニメ

        tweens2restart[3] = DOVirtual.DelayedCall(0.1f, () => {
            kemuri.sprite = kemuriSprites[1];
        });
        tweens2restart[4] = DOVirtual.DelayedCall(0.2f, () => {
            kemuri.sprite = kemuriSprites[2];
        });

        tweens2restart[5] = DOVirtual.DelayedCall(0.3f, () => {
            kemuri.sprite = kemuriSprites[3];
            tweensNot2restart[0] = kemuri.transform.DOLocalMoveX(-50, 0.7f);
        });

        tweens2restart[6] = DOVirtual.DelayedCall(0.4f, () => {
            tweensNot2restart[1] = kemuri.DOFade(0, 0.6f);
        });

        //おわり

        tweens2restart[7] = DOVirtual.DelayedCall(1f, () => {
            gameObject.SetActive(false);
            InUse = false;
        });

        foreach(Tween tw in tweens2restart) tw.SetAutoKill(false).Complete();
        foreach(Tween tw in tweensNot2restart) tw.Complete();
    }

    public void Activate(string heroDirStr){

        InUse = true;
        transform.position = HeroDefiner.CurrentHeroPos + new Vector3(
            heroDirStr=="r" ? positionFromHero.x : - positionFromHero.x,
            positionFromHero.y,
            positionFromHero.z
        );
        transform.localScale = new Vector3(heroDirStr=="r" ? 1 : -1, 1, 1);
        tsuchi.sprite = tsuchiSprites[0];
        kemuri.sprite = kemuriSprites[0];
        kemuri.color = new Color(1,1,1, 0.5f);
        kemuri.transform.localPosition = new Vector3(0,0,0);
        foreach(Tween tw in tweens2restart){
            tw.Rewind();
            tw.Restart();
        }
        foreach(Tween tw in tweensNot2restart) tw.Kill();
    }

    public bool InUse{ get; private set; }
}
