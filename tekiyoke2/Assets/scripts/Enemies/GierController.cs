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

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
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
}
