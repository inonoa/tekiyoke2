﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GierController : EnemyController
{
    enum GierState{ BeforeFindingR, BeforeFindingL, FindingNow, Running }
    GierState state;
    int findingCount = 0;

    [SerializeField] float walkSpeed = 2;

    [SerializeField] float runSpeed = 5;

    [SerializeField] float distanceToFindHero = 200;
    [SerializeField] float distanceToMissHero = 500;

    [SerializeField] float framesBeforeRun = 60;
    [SerializeField] float jumpForce = 500;
    [SerializeField] bool toRightFirst = true;

    GroundChecker groundChecker;

    [SerializeField] SpriteRenderer HontaiSR = null;
    [SerializeField] Vector3 normalRotateSpeed = Vector3.zero;
    [SerializeField] Vector3 runningRotateSpeed = Vector3.zero;
    [SerializeField] SpriteRenderer eyeRenderer;
    [SerializeField] Sprite eyeNormal;
    [SerializeField] Sprite eyeFinding;
    [SerializeField] Sprite eyeRunning;

    void Start()
    {
        base.Init();
        HeroDefiner.currentHero.jumped += HeroJumped;
        groundChecker = transform.Find("GroundChecker").GetComponent<GroundChecker>();
        transform.Find("DontWannaFallR").GetComponent<DontWannaFall>().about2fall += Turn;
        transform.Find("DontWannaFallL").GetComponent<DontWannaFall>().about2fall += Turn;
        transform.Find("Collider2Wall").GetComponent<Collider2Wall>().touched2Wall += Turn;
        state = toRightFirst ? GierState.BeforeFindingR : GierState.BeforeFindingL;
    }

    new void Update()
    {
        base.Update_();

        switch(state){

            case GierState.BeforeFindingR:
                if( NearHero() ) Find();

                MoveX_ConsideringGravity(walkSpeed);
                HontaiSR.transform.Rotate(-normalRotateSpeed);
                break;

            case GierState.BeforeFindingL:
                if( NearHero() ) Find();
                
                MoveX_ConsideringGravity(-walkSpeed);
                HontaiSR.transform.Rotate(normalRotateSpeed);
            break;

            case GierState.FindingNow:
                findingCount ++;
                if(findingCount > framesBeforeRun){
                    state = GierState.Running;
                    eyeRenderer.sprite = eyeRunning;
                    findingCount = 0;
                }
                break;

            case GierState.Running:
                if(HeroDefiner.CurrentHeroPos.x > transform.position.x + 10){
                    MoveX_ConsideringGravity( runSpeed);
                    HontaiSR.transform.Rotate(-runningRotateSpeed);
                    eyeRenderer.flipX = false;
                }
                if(HeroDefiner.CurrentHeroPos.x < transform.position.x - 10){
                    MoveX_ConsideringGravity(-runSpeed);
                    HontaiSR.transform.Rotate(runningRotateSpeed);
                    eyeRenderer.flipX = true;
                }

                if( MyMath.DistanceXY(HeroDefiner.CurrentHeroPos,transform.position) > distanceToMissHero ){
                    if(rBody.velocity.x > 0) state = GierState.BeforeFindingR;
                    else                     state = GierState.BeforeFindingL;
                    eyeRenderer.sprite = eyeNormal;
                }
                break;
        }
    }

    bool NearHero(){
        //なんでColliderつけてないんだこれ… -> 判定の広さ調整が若干楽になるとかか(融通は利かないけど…)
        return MyMath.DistanceXY(HeroDefiner.CurrentHeroPos,transform.position) < distanceToFindHero;
    }

    void Find(){
        state = GierState.FindingNow;
        eyeRenderer.sprite = eyeFinding;
        eyeRenderer.flipX = HeroDefiner.CurrentHeroPos.x < transform.position.x;
    }

    void HeroJumped(object sender, EventArgs e){
        if(((HeroJumpedEventArgs)e).isKick) return;
        if(groundChecker.IsOnGround && state == GierState.Running) rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
    }

    void Turn(object sender, EventArgs e){
        if(state==GierState.BeforeFindingL){
            state = GierState.BeforeFindingR;
            eyeRenderer.flipX = false;
        }
        else if(state==GierState.BeforeFindingR){
            state = GierState.BeforeFindingL;
            eyeRenderer.flipX = true;
        }
    }
}
