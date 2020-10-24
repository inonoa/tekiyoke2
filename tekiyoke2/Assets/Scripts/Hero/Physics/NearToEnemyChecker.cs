using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearToEnemyChecker : MonoBehaviour
{
    static readonly float timeScaleWhenNear = 0.3f;

    [SerializeField]
    bool slowNearEnemy = true;

    bool nearEnemy = false;

    [SerializeField]
    int DPInterval = 3;
    int DPCount = 0;

    void Update()
    {
        if(nearEnemy){
            if(DPCount==0){
                //DPManager.Instance.AddDP(1);
            }
            DPCount ++;
            DPCount %= DPInterval;
        }else{
            DPCount = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Enemy"){
            if(slowNearEnemy) TimeManager.CurrentInstance.SetTimeScale(0.2f);
            nearEnemy = true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag=="Enemy"){
            if(slowNearEnemy) TimeManager.CurrentInstance.SetTimeScale(1);
            nearEnemy = false;
        }
    }
}
