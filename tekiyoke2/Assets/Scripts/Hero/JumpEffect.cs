using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//絶対に便利クラスを作ったほうがいい
public class JumpEffect : MonoBehaviour, IReusable
{
    public bool InUse{ get; private set; } = false;

    [SerializeField] Vector3 positionFromHero;

    [SerializeField] SpriteRenderer tsuchi;
    [SerializeField] Sprite[] tsuchiSprites;
    [SerializeField] SpriteRenderer kaze;
    [SerializeField] Sprite[] kazeSprites;

    Tween[] tweensToRestart = new Tween[3];
    Tween[] tweensNotToRestart = new Tween[0];

    public void Activate(string heroDirStr){
        InUse = true;

        tsuchi.sprite = tsuchiSprites[0];
        kaze.sprite = kazeSprites[0];
        transform.position = HeroDefiner.CurrentHeroPos + positionFromHero;

        foreach(Tween tw in tweensToRestart) tw.Restart();
        foreach(Tween tw in tweensNotToRestart) tw.Kill();
    }


    void Awake()
    {
        tweensToRestart[0] = DOVirtual.DelayedCall(0.15f, ()=>{
            tsuchi.sprite = tsuchiSprites[1];
            kaze.sprite = kazeSprites[1];
        });

        tweensToRestart[1] = DOVirtual.DelayedCall(0.3f, ()=>{
            tsuchi.sprite = tsuchiSprites[2];
            kaze.sprite = kazeSprites[2];
        });

        tweensToRestart[2] = DOVirtual.DelayedCall(0.45f, ()=>{
            InUse = false;
            gameObject.SetActive(false);
        });

        foreach(Tween tw in tweensToRestart) tw.SetAutoKill(false).Complete();
    }
}
