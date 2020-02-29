using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennerController : EnemyController
{
    enum State{ Wait, Jump, Shoot, Rest }
    State state = State.Wait;

    bool _EyeToRight;
    bool EyeToRight{
        get { return _EyeToRight; }
        set {
            if( _EyeToRight && !value) { }
            if(!_EyeToRight &&  value) { }

            _EyeToRight = value;
        }
    }

    [Header("--Kennerのパラメータ？--")]

    [SerializeField] int restFrames = 100;
    int restFramesNow = 0;

    [SerializeField] float distanceToFindHero = 200;
    [SerializeField] float jumpForce = 400;


    [Header("--弾の撃ち方--")]
    [SerializeField] int framesPerShoot = 20;
    int framesToShootNow = 1;

    [SerializeField] int howManyShoots = 3;
    int howManyShootsNow = 0;

    [SerializeField] float upAngle = 10;
    [SerializeField] float downAngle = 70;
    [SerializeField] float num_tamaPerShoot = 5;

    GroundChecker groundChecker;

    [Header("--弾のパラメータ--")]
    [SerializeField] GameObject tama = null;

    [SerializeField] float tamaSpeed = 10;

    [SerializeField] int tamaLife = 100;

    new void Start()
    {
        base.Start();
        groundChecker = transform.Find("GroundChecker").GetComponent<GroundChecker>();
    }

    new void Update()
    {
        EyeToRight = transform.position.x < HeroDefiner.CurrentHeroPos.x;

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
            Vector3 offset = EyeToRight ? new Vector3(60,-50) : new Vector3(-60,-50);
            GameObject imatama = Instantiate(tama, transform.position + offset, Quaternion.identity, transform.parent);

            float angle = - upAngle - i * (downAngle - upAngle) / (num_tamaPerShoot - 1);
            if(!EyeToRight) angle = - 180 - angle;
            imatama.GetComponent<TamaController>().angle = angle;
            imatama.transform.Rotate(new Vector3(0,0,angle));

            imatama.GetComponent<TamaController>().speed = tamaSpeed;
            imatama.GetComponent<TamaController>().life = tamaLife;
        }
    }
}
