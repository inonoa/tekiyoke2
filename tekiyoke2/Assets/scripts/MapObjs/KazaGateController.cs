using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KazaGateController : MonoBehaviour
{
    [SerializeField] Transform gate;
    SpriteRenderer spRenderer;
    BoxCollider2D col;
    [SerializeField] float openVel = 1;
    Vector3 defPos;
    bool isOpen = false;
    void Start()
    {
        GetComponent<KazagurumaObserver>().AllRotated += (s, e) =>
        {
            if(!isOpen){
                StartCoroutine("Open");
                isOpen = true;
            }
        };
        spRenderer = gate.GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        defPos = gate.position;
    }


    IEnumerator Open(){

        while(true){

            gate.position += Vector3.up * openVel;
            //当たり判定縮めてる
            col.offset += Vector2.up * openVel / 2;
            col.size -= new Vector2(0, 1) * openVel;

            if(gate.position.y >= defPos.y + 100){
                gate.position = new Vector3(gate.position.x, defPos.y + 100, gate.position.z);
                col.enabled = false;
                yield break;
            }

            yield return null;
        }
    }

}
