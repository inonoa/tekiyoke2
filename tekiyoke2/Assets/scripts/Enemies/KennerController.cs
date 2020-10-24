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

    [SerializeField] float restSeconds = 1;
    float restSecondsNow = 0;

    [SerializeField] float distanceToFindHero = 200;
    [SerializeField] float jumpForce = 400;
    [SerializeField] float gravity = 9.8f;


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

    [SerializeField] float tamaLifeSeconds = 1.5f;
    [Header("--子オブジェクト類--")]
    [SerializeField] GroundChecker groundChecker = null;
    [SerializeField] Transform gazosTF = null;
    [SerializeField] Transform baneTF = null;
    [SerializeField] Transform dodaiTF = null;
    [SerializeField] SpriteRenderer hontaiSR = null;
    [SerializeField] Sprite hontaiSpriteActive = null;
    [SerializeField] Sprite hontaiSpriteInactive = null;

    static bool inNewScene; //なんかあほらしいな……
    static ObjectPool<TamaController> tamaPool;
    SoundGroup soundGroup;

    void Awake(){
        inNewScene = true;
    }

    void Start()
    {
        base.Init();
        if(inNewScene){
            //これがどのタイミングで呼ばれるのか分からん(主人公が近づいてきてから呼ばれてたらつらい)
            tamaPool = new ObjectPool<TamaController>(tama, 128, DraftManager.CurrentInstance.GameMasterTF);
            inNewScene = false;
        }
        soundGroup = GetComponent<SoundGroup>();
    }

    new void Update()
    {
        if(groundChecker.IsOnGround) EyeToRight = transform.position.x < HeroDefiner.CurrentHeroPos.x;

        switch(state)
        {
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
                    baneTF.DOScaleY(0.3f, 0.5f).FollowTimeScale(aroundHero: false);
                    dodaiTF.DOLocalMoveY(-23,0.5f).FollowTimeScale(aroundHero: false);
                }
                break;
            
            case State.Shoot:
                framesToShootNow --;

                if(framesToShootNow==0){
                    Shoot();
                    framesToShootNow = framesPerShoot;

                    howManyShootsNow --;
                    if(howManyShootsNow==0){
                        state = State.Rest;
                    }
                }
                break;
            
            case State.Rest:
                restSecondsNow += TimeManager.DeltaTimeExceptHero;

                if(groundChecker.IsOnGround) rBody.velocity = new Vector2();

                if(restSecondsNow >= restSeconds)
                {
                    restSecondsNow = 0f;
                    if(MyMath.DistanceXY(transform.position, HeroDefiner.CurrentHeroPos) < distanceToFindHero){
                        Jump();
                    }
                    else{
                        state = State.Wait;
                        hontaiSR.sprite = hontaiSpriteInactive;
                    }
                }

                break;
        }
    }

    void FixedUpdate()
    {
        if(state == State.Shoot || groundChecker.IsOnGround) return;

        rBody.velocity -= new Vector2(0, gravity * TimeManager.FixedDeltaTimeExceptHero);
    }

    void Jump()
    {
        rBody.velocity = new Vector2(0, jumpForce);
        state = State.Jump;
        baneTF.DOScaleY(1, 0.3f).SetEase(Ease.InOutSine).FollowTimeScale(aroundHero: false);
        dodaiTF.DOLocalMoveY(-46,0.3f).SetEase(Ease.InOutSine).FollowTimeScale(aroundHero: false);
        soundGroup.Play("Jump");
    }

    void Shoot(){
        for(int i=0; i<num_tamaPerShoot; i++){

            float angle = - upAngle - i * (downAngle - upAngle) / (num_tamaPerShoot - 1);
            if(!EyeToRight) angle = - 180 - angle;
            TamaController imatama = tamaPool.ActivateOne(
                angle.ToString()
                + " " + tamaSpeed.ToString()
                + " " + tamaLifeSeconds.ToString()
            );
            
            Vector3 offset = EyeToRight ? new Vector3(70,-30) : new Vector3(-70,-30);
            imatama.transform.position = transform.position + offset;
        }

        soundGroup.Play("Shoot");
        rBody.velocity = Vector2.zero;
    }
}
