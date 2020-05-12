using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class KazaGateController : MonoBehaviour
{
    [SerializeField] Transform gate;
    SpriteRenderer spRenderer;
    SoundGroup soundGroup;
    BoxCollider2D col;
    [SerializeField] float openVel = 1;
    [SerializeField] float closeVel = 2;
    Vector3 defPos;
    enum State{ Wait, Opening, Open, Closing }
    State state = State.Wait;
    void Start()
    {
        soundGroup = GetComponent<SoundGroup>();
        KazagurumaObserver ko = GetComponent<KazagurumaObserver>();
        ko.AllRotated += (s, e) => {
            soundGroup.Play("Move");
            state = State.Opening;
        };
        ko.NotAllRotated += (s, e) => {
            if(state==State.Opening){
                soundGroup.Stop("Move");
                soundGroup.Play("Stop");
                DOVirtual.DelayedCall(0.1f, () => soundGroup.Play("Move"));
            }else{
                soundGroup.Play("Move");
            }
            state = State.Closing;
            col.enabled = true;
        };
        spRenderer = gate.GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        defPos = gate.position;
    }

    void Update(){

        switch(state){

            case State.Wait:
                break;
            
            case State.Opening:

                gate.position += Vector3.up * openVel;
                //当たり判定縮めてる
                col.offset += Vector2.up * openVel / 2;
                col.size -= new Vector2(0, 1) * openVel;

                if(gate.position.y >= defPos.y + 100){
                    gate.position = new Vector3(gate.position.x, defPos.y + 100, gate.position.z);
                    col.enabled = false;
                    state = State.Open;
                    soundGroup.Stop("Move");
                    soundGroup.Play("Stop");
                }
                break;
            
            case State.Open:

                break;
            
            case State.Closing:

                gate.position -= Vector3.up * closeVel;
                //当たり判定広げてる
                col.offset -= Vector2.up * closeVel / 2;
                col.size += new Vector2(0, 1) * closeVel;

                if(gate.position.y <= defPos.y){
                    gate.position = new Vector3(gate.position.x, defPos.y, gate.position.z);
                    state = State.Wait;
                    soundGroup.Stop("Move");
                    soundGroup.Play("Stop");
                }
                break;
        }
    }

}
