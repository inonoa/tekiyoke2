using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GetDPinEnemy : MonoBehaviour
{
    public event EventHandler gotDP;

    ///<summary>敵のソウル的なのからDPを奪う、光ってからフェードアウトする</summary>
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Enemy"){
            print("dpppp");
            var dPinEnemy = other.GetComponentInParent<EnemyController>().DPCD;
            dPinEnemy.Light();
            DOVirtual.DelayedCall(1f,() => dPinEnemy.FadeOut());
            gotDP?.Invoke(dPinEnemy.CollectDP(), EventArgs.Empty);
        }
    }
}
