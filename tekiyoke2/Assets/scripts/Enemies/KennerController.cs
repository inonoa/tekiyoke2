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

    [SerializeField]
    int framesPerShoot = 20;
    int framesToShootNow = 1;

    [SerializeField]
    int howManyShoots = 3;
    int howManyShootsNow = 0;

    [SerializeField]
    float upAngle = 10;
    [SerializeField]
    float downAngle = 70;
    [SerializeField]
    float num_tamaPerShoot = 5;

    GroundChecker groundChecker;
    [SerializeField]
    GameObject tama = null;

    [SerializeField]
    float tamaSpeed = 10;

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
                if(rBody.velocity.y < 0){
                    state = State.Shoot;
                    framesToShootNow = 1;
                    howManyShootsNow = howManyShoots;
                }
                break;
            
            case State.Shoot:
                rBody.simulated = false;
                framesToShootNow --;

                if(framesToShootNow==0){
                    Shoot();
                    framesToShootNow = framesPerShoot;

                    howManyShootsNow --;
                    if(howManyShootsNow==0){
                        state = State.Rest;
                        rBody.simulated = true;
                    }
                }
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

    void Shoot(){
        for(int i=0; i<num_tamaPerShoot; i++){
            GameObject imatama = Instantiate(tama,transform.position + new Vector3(60,-50),Quaternion.identity,transform.parent);

            float angle = - upAngle - i * (downAngle - upAngle) / (num_tamaPerShoot - 1);
            imatama.GetComponent<TamaController>().angle = angle;
            imatama.GetComponent<TamaController>().speed = tamaSpeed;
            imatama.transform.Rotate(new Vector3(0,0,angle));
        }
    }
}
