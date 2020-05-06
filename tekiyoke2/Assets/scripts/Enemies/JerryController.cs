using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;

public class JerryController : EnemyController
{

    float amplitude;

    [SerializeField] float speedRate = 4;

    float centerPositionY;

    [SerializeField] bool IsGoingUp = true;

    float JellyPosY{ get => rBody.transform.position.y; }

    static readonly float speedYEpsilon = 0.01f;
    static readonly float linear2Sin = 80;

    [SerializeField] int lightFrames = 10;
    [SerializeField] int unlightFrames = 30;

    [SerializeField] SpriteRenderer kasaSR;
    [SerializeField] SpriteRenderer asiSR;
    [SerializeField] Sprite kasaSpriteUp;
    [SerializeField] Sprite asiSpriteUp;
    [SerializeField] Sprite kasaSpriteDown;
    [SerializeField] Sprite asiSpriteDown;
    [SerializeField] SpriteRenderer lightSR;
    [SerializeField] SoundGroup soundGroup;

    //定数シュッと置いとく方法を探している
    static readonly (string _Volume, string Kaze) c = ("_Volume", "Kaze");


    new void Start(){
        rBody = transform.Find("Kasa").GetComponent<Rigidbody2D>();

        float posU = transform.Find("PositionU").position.y;
        float posD = transform.Find("PositionD").position.y;
        centerPositionY = (posU + posD) / 2;
        amplitude       = (posU - posD) / 2;

        kasaSR.sprite = IsGoingUp ? kasaSpriteUp : kasaSpriteDown;
        asiSR.sprite = IsGoingUp ? asiSpriteUp : asiSpriteDown;
        lightSR.material.SetFloat(c._Volume, IsGoingUp ? 1 : 0);
    }

    new void Update()
    {
        if(IsGoingUp){
            if(JellyPosY > centerPositionY+amplitude-linear2Sin){
                //上端
                float v = (float)Math.Sqrt(Math.Max(centerPositionY+amplitude - JellyPosY, speedYEpsilon)) * speedRate/10;
                base.MovePos(0,v);

                if(JellyPosY >= centerPositionY+amplitude-1){
                    IsGoingUp = false;
                    kasaSR.sprite = kasaSpriteDown;
                    asiSR.sprite = asiSpriteDown;
                    StartCoroutine("Unlight");
                }

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

                if(JellyPosY <= centerPositionY-amplitude+1){
                    IsGoingUp = true;
                    kasaSR.sprite = kasaSpriteUp;
                    asiSR.sprite = asiSpriteUp;
                    StartCoroutine("Light");
                    if(MyMath.DistanceXY(transform.position, HeroDefiner.CurrentHeroPos) < 625) soundGroup.Play(c.Kaze);
                }
            }
        }
    }

    IEnumerator Light(){

        for(int i=0;i<lightFrames;i++){
            lightSR.material.SetFloat(c._Volume, (i+1)/(float)lightFrames);
            yield return null;
        }
    }
    IEnumerator Unlight(){

        for(int i=0;i<unlightFrames;i++){
            lightSR.material.SetFloat(c._Volume, (unlightFrames-1-i)/(float)unlightFrames);
            yield return null;
        }
    }
}
