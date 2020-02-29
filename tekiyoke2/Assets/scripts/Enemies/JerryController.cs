﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;

public class JerryController : EnemyController
{

    float amplitude;

    [SerializeField]
    float speedRate = 4;

    float centerPositionY;

    public bool isGoingUp = true;

    float JellyPosY{ get => rBody.transform.position.y; }

    static readonly float speedYEpsilon = 0.01f;
    static readonly float linear2Sin = 80;

    new void Start(){
        rBody = transform.Find("Jelly").GetComponent<Rigidbody2D>();

        float posU = transform.Find("PositionU").position.y;
        float posD = transform.Find("PositionD").position.y;
        centerPositionY = (posU + posD) / 2;
        amplitude       = (posU - posD) / 2;
    }

    new void Update()
    {
        if(isGoingUp){
            if(JellyPosY > centerPositionY+amplitude-linear2Sin){
                //上端
                float v = (float)Math.Sqrt(Math.Max(centerPositionY+amplitude - JellyPosY, speedYEpsilon)) * speedRate/10;
                base.MovePos(0,v);

                if(JellyPosY >= centerPositionY+amplitude-1) isGoingUp = false;

            }else if(JellyPosY > centerPositionY-amplitude+linear2Sin){
                //中間
                base.MovePos(0,speedRate);

            }else{
                //下端
                float v = (float)Math.Sqrt(Math.Max(JellyPosY - centerPositionY+amplitude, speedYEpsilon)) * speedRate/10;
                base.MovePos(0,v);

            }
        }else{
            if(JellyPosY > centerPositionY+amplitude-linear2Sin){
                //上端
                float v = (float)Math.Sqrt(Math.Max(centerPositionY+amplitude - JellyPosY, speedYEpsilon)) * speedRate/10;
                base.MovePos(0,-v);

            }else if(JellyPosY > centerPositionY-amplitude+linear2Sin){
                //中間
                base.MovePos(0,-speedRate);

            }else{
                //下端
                float v = (float)Math.Sqrt(Math.Max(JellyPosY - centerPositionY+amplitude, speedYEpsilon)) * speedRate/10;
                base.MovePos(0,-v);

                if(JellyPosY <= centerPositionY-amplitude+1) isGoingUp = true;
            }
        }
    }
}
