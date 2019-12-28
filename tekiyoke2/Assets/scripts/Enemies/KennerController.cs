using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennerController : EnemyController
{
    enum State{ Wait, Jump, Shoot, Rest }
    State state = State.Wait;

    [SerializeField]
    int restFrames = 100;
    int restFramesNow = 0;

    [SerializeField]
    float distanceToFindHero = 200;
    [SerializeField]
    float jumpForce = 400;

    GroundChecker groundChecker;

    new void Start()
    {
        base.Start();
        groundChecker = transform.Find("GroundChecker").GetComponent<GroundChecker>();
    }

    // Update is called once per frame
    new void Update()
    {
        switch(state){

            case State.Wait:
                if(MyMath.DistanceXY(transform.position, HeroDefiner.CurrentHeroPos) < distanceToFindHero){
                    rBody.velocity = new Vector2(0,jumpForce);
                    state = State.Jump;
                }
                break;
            
            case State.Jump:
                if(rBody.velocity.y < 0) state = State.Shoot;
                break;
            
            case State.Shoot:
                //todo
                state = State.Rest;
                break;
            
            case State.Rest:
                restFramesNow ++;

                if(restFramesNow==restFrames){
                    restFramesNow = 0;
                    if(MyMath.DistanceXY(transform.position, HeroDefiner.CurrentHeroPos) < distanceToFindHero){
                        rBody.velocity = new Vector2(0,jumpForce);
                        state = State.Jump;
                    }
                    else state = State.Wait;

                }else if(groundChecker.IsOnGround) rBody.velocity = new Vector2();

                break;
        }
    }
}
