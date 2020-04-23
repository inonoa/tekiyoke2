using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GetDPinEnemy : MonoBehaviour
{
    public event EventHandler gotDP;

    [SerializeField] int freezeFrames = 7;

    HeroMover hero;
    BoxCollider2D col;

    ///<summary>敵のソウル的なのからDPを奪う、光ってからフェードアウトする</summary>
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Enemy"){
            print("dpppp");
            var dPinEnemy = other.GetComponentInParent<EnemyController>().DPCD;
            if(dPinEnemy.IsActive){
                dPinEnemy.Light();
                StartCoroutine(FreezeAndMelt());
                DOVirtual.DelayedCall(1f,() => dPinEnemy.FadeOut());
                float dp = dPinEnemy.CollectDP();
                gotDP?.Invoke(dp, EventArgs.Empty);
            }
        }
    }

    IEnumerator FreezeAndMelt(){
        yield return null;

        Tokitome.SetTime(0);
        for(int i=0; i<freezeFrames-1; i++){
            yield return null;
        }
        Tokitome.SetTime(1);
    }

    void Start(){
        hero = GetComponentInParent<HeroMover>();
        col = GetComponent<BoxCollider2D>();
    }

    void Update(){
        Vector2 lastPos = HeroDefiner.CurrentHeroPos;
        Vector2 currentPos = HeroDefiner.CurrentHeroExpectedPos;
        Vector2 posDist = currentPos - lastPos;

        col.offset = -new Vector2(posDist.x / 2, 0);
        col.size = new Vector2(Mathf.Abs(posDist.x), 30);
    }
}
