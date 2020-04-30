using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KennerController : EnemyController
{
    enum State{ Wait, Jump, Shoot, Rest }
    State state = State.Wait;

    bool _EyeToRight;
    bool EyeToRight{
        get { return _EyeToRight; }
        set {
            if(value != _EyeToRight){
                gazosTF.localScale = new Vector3( value ? 1 : -1, 1, 1);
            }
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

    [Header("--弾のパラメータ--")]
    [SerializeField] TamaController tama = null;

    [SerializeField] float tamaSpeed = 10;

    [SerializeField] int tamaLife = 100;
    [Header("--子オブジェクト類--")]
    [SerializeField] GroundChecker groundChecker = null;
    [SerializeField] Transform gazosTF = null;
    [SerializeField] Transform baneTF = null;
    [SerializeField] Transform dodaiTF = null;
    [SerializeField] SpriteRenderer hontaiSR = null;
    [SerializeField] Sprite hontaiSpriteActive = null;
    [SerializeField] Sprite hontaiSpriteInactive = null;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        if(groundChecker.IsOnGround) EyeToRight = transform.position.x < HeroDefiner.CurrentHeroPos.x;

        switch(state){

            case State.Wait:
                if(MyMath.DistanceXY(transform.position, HeroDefiner.CurrentHeroPos) < distanceToFindHero){
                    Jump();
                    hontaiSR.sprite = hontaiSpriteActive;
                }
                break;
            
            case State.Jump:
                if(rBody.velocity.y < 0){
                    state = State.Shoot;
                    framesToShootNow = 1;
                    howManyShootsNow = howManyShoots;
                    baneTF.DOScaleY(0.3f, 0.5f);
                    dodaiTF.DOLocalMoveY(-23,0.5f);
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
                        Jump();
                    }
                    else{
                        state = State.Wait;
                        hontaiSR.sprite = hontaiSpriteInactive;
                    }

                }else if(groundChecker.IsOnGround) rBody.velocity = new Vector2();

                break;
        }
    }

    void Jump(){
        rBody.velocity = new Vector2(0,jumpForce);
        state = State.Jump;
        baneTF.DOScaleY(1, 0.3f).SetEase(Ease.InOutSine);
        dodaiTF.DOLocalMoveY(-46,0.3f).SetEase(Ease.InOutSine);
    }

    void Shoot(){
        for(int i=0; i<num_tamaPerShoot; i++){
            Vector3 offset = EyeToRight ? new Vector3(70,-30) : new Vector3(-70,-30);
            TamaController imatama = Instantiate(tama, transform.position + offset, Quaternion.identity, DraftManager.CurrentInstance.GameMasterTF);

            float angle = - upAngle - i * (downAngle - upAngle) / (num_tamaPerShoot - 1);
            if(!EyeToRight) angle = - 180 - angle;
            imatama.angle = angle;
            imatama.transform.Rotate(new Vector3(0,0,angle));

            imatama.speed = tamaSpeed;
            imatama.life = tamaLife;
        }
    }
}
