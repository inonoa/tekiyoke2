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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(nearEnemy){
            if(DPCount==0) DPManager.Instance.AddDP(1);
            DPCount ++;
            DPCount %= DPInterval;
        }else{
            DPCount = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Enemy"){
            if(slowNearEnemy) Tokitome.SetTime(timeScaleWhenNear);
            nearEnemy = true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag=="Enemy"){
            if(slowNearEnemy) Tokitome.SetTime(1);
            nearEnemy = false;
        }
    }
}
