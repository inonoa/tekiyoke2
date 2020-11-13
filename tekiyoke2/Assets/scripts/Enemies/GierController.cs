using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using Sirenix.OdinInspector;

public class GierController : EnemyController, IHaveDPinEnemy
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
    [SerializeField] Vector3 normalRotateSpeed  = Vector3.zero;
    [SerializeField] Vector3 runningRotateSpeed = Vector3.zero;
    [SerializeField] SpriteRenderer eyeRenderer;
    [SerializeField] Sprite eyeNormal;
    [SerializeField] Sprite eyeFinding;
    [SerializeField] Sprite eyeRunning;

    [Space(10)]
    [SerializeField] Rigidbody2D RigidBody;

    [field: SerializeField, LabelText("DPCD")]
    public DPinEnemy DPCD{ get; private set; }

    void Start()
    {
        base.Init();
        HeroDefiner.currentHero.OnJumped.Subscribe(jump => HeroJumped(jump.isKick));
        groundChecker = transform.Find("GroundChecker").GetComponent<GroundChecker>();
        transform.Find("DontWannaFallR").GetComponent<DontWannaFall>().about2fall += Turn;
        transform.Find("DontWannaFallL").GetComponent<DontWannaFall>().about2fall += Turn;
        transform.Find("Collider2Wall").GetComponent<Collider2Wall>().touched2Wall += Turn;
        state = toRightFirst ? GierState.BeforeFindingR : GierState.BeforeFindingL;
    }

    void Update()
    {
        RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);

        switch(state){

            case GierState.BeforeFindingR:
                if( NearHero() ) Find();

                RigidBody.MoveX_ConsideringGravity(walkSpeed);
                HontaiSR.transform.Rotate(-normalRotateSpeed);
                break;

            case GierState.BeforeFindingL:
                if( NearHero() ) Find();
                
                RigidBody.MoveX_ConsideringGravity(-walkSpeed);
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
                    RigidBody.MoveX_ConsideringGravity( runSpeed);
                    HontaiSR.transform.Rotate(-runningRotateSpeed);
                    eyeRenderer.flipX = false;
                }
                if(HeroDefiner.CurrentHeroPos.x < transform.position.x - 10){
                    RigidBody.MoveX_ConsideringGravity(-runSpeed);
                    HontaiSR.transform.Rotate(runningRotateSpeed);
                    eyeRenderer.flipX = true;
                }

                if( MyMath.DistanceXY(HeroDefiner.CurrentHeroPos,transform.position) > distanceToMissHero ){
                    if(RigidBody.velocity.x > 0) state = GierState.BeforeFindingR;
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

    void HeroJumped(bool isKick){
        if(isKick) return;
        if(groundChecker.IsOnGround && state == GierState.Running) RigidBody.velocity = new Vector2(RigidBody.velocity.x, jumpForce);
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
