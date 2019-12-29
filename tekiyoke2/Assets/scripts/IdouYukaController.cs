﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdouYukaController : MonoBehaviour
{
    Vector3 positionA;
    Vector3 positionB;
    Rigidbody2D yukaRB;
    Transform yukaTF;

    enum State{ AtoB, B, BtoA, A }
    State state = State.AtoB;

    [SerializeField]
    float moveSpeed = 10;
    Vector3 moveVec = new Vector3();

    [SerializeField]
    int StopFrames = 60;
    int frames2StopNow = 0;

    [SerializeField]
    ContactFilter2D filter2Hero = new ContactFilter2D();
    Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        positionA = transform.Find("PositionA").position;
        positionB = transform.Find("PositionB").position;
        yukaTF    = transform.Find("Yuka");
        yukaRB    = yukaTF.GetComponent<Rigidbody2D>();
        col       = yukaTF.GetComponent<Collider2D>();

        moveVec = ( positionB - positionA ).normalized * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        switch(state){

            case State.AtoB:
                if(MyMath.DistanceXY(yukaTF.position, positionB) < 10){
                    yukaRB.MovePosition(positionB);
                    frames2StopNow = StopFrames;
                    state = State.B;
                }else{
                    yukaRB.MovePosition(yukaTF.position + moveVec);
                }
                break;

            case State.B:
                frames2StopNow --;
                if(frames2StopNow==0) state = State.BtoA;
                break;

            case State.BtoA:
                if(MyMath.DistanceXY(yukaTF.position, positionA) < 10){
                    yukaRB.MovePosition(positionA);
                    frames2StopNow = StopFrames;
                    state = State.A;
                }else{
                    yukaRB.MovePosition(yukaTF.position - moveVec);
                }
                break;

            case State.A:
                frames2StopNow --;
                if(frames2StopNow==0) state = State.AtoB;
                break;
        }
    }
}
