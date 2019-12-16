using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GierController : EnemyController
{
    //あとでクラス化するか…？
    enum GierState{ BeforeFinding, FindingNow, Running }
    GierState state = GierState.BeforeFinding;
    int findingCount = 0;

    [SerializeField]
    float runSpeed = 5;


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
                if( MyMath.DistanceXY(HeroDefiner.CurrentHeroPos,transform.position) > 500 ){
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
                if(HeroDefiner.CurrentHeroPos.x > transform.position.x) MovePos(runSpeed,0);
                else MovePos(-runSpeed,0);
                break;
        }
    }
}
