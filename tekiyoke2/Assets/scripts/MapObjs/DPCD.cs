using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPCD : MonoBehaviour
{
    [SerializeField] int rotateInterval = 10;
    int rotateCount = 0;
    [SerializeField] float rotateSpeed = 20;
    [SerializeField] int DPperDPCD = 1;

    void Update()
    {
        rotateCount ++;
        rotateCount %= rotateInterval;
        if(rotateCount==0) transform.Rotate(new Vector3(0,0,rotateSpeed));
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Destroy(gameObject);
            DPManager.Instance.AddDP(DPperDPCD);
        }
    }
}
