using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GierController : EnemyController
{
    enum GierState{ BeforeFindingR, BeforeFindingL, FindingNow, Running }
    GierState state = GierState.BeforeFindingR;
    int findingCount = 0;

    [SerializeField]
    float walkSpeed = 2;

    [SerializeField]
    float runSpeed = 5;

    [SerializeField]
    float distanceToFindHero = 200;
    [SerializeField]
    float distanceToMissHero = 500;

    [SerializeField]
    float framesBeforeRun = 60;
    [SerializeField]
    float jumpForce = 500;

    GroundChecker groundChecker;

    [SerializeField]
    ContactFilter2D filter;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        HeroDefiner.currentHero.jumped += HeroJumped;
        groundChecker = transform.Find("GroundChecker").GetComponent<GroundChecker>();
        transform.Find("DontWannaFallR").GetComponent<DontWannaFall>().about2fall += Turn;
        transform.Find("DontWannaFallL").GetComponent<DontWannaFall>().about2fall += Turn;
        transform.Find("Collider2Wall").GetComponent<EnemyCollider2Wall>().touched2Wall += Turn;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        switch(state){

            case GierState.BeforeFindingR:
                //なんでColliderつけてないんだこれ… -> 判定の広さ調整が若干楽になるとかか(融通は利かないけど…)
                if( MyMath.DistanceXY(HeroDefiner.CurrentHeroPos,transform.position) < distanceToFindHero ){
                    state = GierState.FindingNow;
                }
                MoveX_ConsideringGravity(walkSpeed);
                break;

            case GierState.BeforeFindingL:
                if( MyMath.DistanceXY(HeroDefiner.CurrentHeroPos,transform.position) < distanceToFindHero ){
                    state = GierState.FindingNow;
                }
                MoveX_ConsideringGravity(-walkSpeed);
            break;

            case GierState.FindingNow:
                findingCount ++;
                if(findingCount > framesBeforeRun){
                    state = GierState.Running;
                    findingCount = 0;
                }
                break;

            case GierState.Running:
                if(HeroDefiner.CurrentHeroPos.x > transform.position.x + 10) MoveX_ConsideringGravity( runSpeed);
                if(HeroDefiner.CurrentHeroPos.x < transform.position.x - 10) MoveX_ConsideringGravity(-runSpeed);

                if( MyMath.DistanceXY(HeroDefiner.CurrentHeroPos,transform.position) > distanceToMissHero ){
                    if(rBody.velocity.x > 0) state = GierState.BeforeFindingR;
                    else                     state = GierState.BeforeFindingL;
                }
                break;
        }
    }

    void HeroJumped(object sender, EventArgs e){
        if(((HeroJumpedEventArgs)e).isKick) return;
        if(groundChecker.IsOnGround && state == GierState.Running) rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
    }

    void Turn(object sender, EventArgs e){
        if(state==GierState.BeforeFindingL) state = GierState.BeforeFindingR;
        else if(state==GierState.BeforeFindingR) state = GierState.BeforeFindingL;
    }
}
