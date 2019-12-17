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
    float fallSpeedMax = 100;

    [SerializeField]
    float distanceToFindHero = 200;


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
                if(findingCount > 80){
                    state = GierState.Running;
                    findingCount = 0;
                }
                break;

            case GierState.Running:
                //多用しそうだし関数化するべきかな
                if(HeroDefiner.CurrentHeroPos.x > transform.position.x) rBody.velocity = new Vector2(runSpeed * Time.timeScale, Math.Max(rBody.velocity.y, -fallSpeedMax));
                else rBody.velocity = new Vector2(-runSpeed * Time.timeScale, Math.Max(rBody.velocity.y, -fallSpeedMax));
                break;
        }
    }
}
