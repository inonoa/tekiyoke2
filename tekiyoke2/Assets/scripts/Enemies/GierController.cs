using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GierController : EnemyController
{
    //あとでクラス化するか…？
    enum GierState{ BeforeFinding, FindingNow, Running }
    GierState state = GierState.BeforeFinding;
    int findingCount = 0;

    [SerializeField]
    float runSpeed = 5;

    [SerializeField]
    float distanceToFindHero = 200;

    [SerializeField]
    float framesBeforeRun = 60;
    [SerializeField]
    float jumpForce = 500;

    GierGroundChecker groundChecker;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        HeroDefiner.currentHero.jumped += HeroJumped;
        groundChecker = transform.Find("GroundChecker").GetComponent<GierGroundChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state){

            case GierState.BeforeFinding:
                if( MyMath.DistanceXY(HeroDefiner.CurrentHeroPos,transform.position) < distanceToFindHero ){
                    state = GierState.FindingNow;
                }
                break;

            case GierState.FindingNow:
                findingCount ++;
                if(findingCount > framesBeforeRun){
                    state = GierState.Running;
                    findingCount = 0;
                }
                break;

            case GierState.Running:
                if(HeroDefiner.CurrentHeroPos.x > transform.position.x) MoveX_ConsideringGravity(runSpeed);
                else MoveX_ConsideringGravity(-runSpeed);
                break;
        }
    }

    void HeroJumped(object sender, EventArgs e){
        Debug.Log("主人公が飛んだ！");
        if(groundChecker.IsOnGround) rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
    }
}
