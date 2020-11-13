using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class KennerController : EnemyController, IHaveDPinEnemy
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

    Vector2 velocity = new Vector2();

    [Header("--Kennerのパラメータ？--")]

    [SerializeField] float restSeconds = 1.6f;
    float restSecondsNow = 0;

    [SerializeField] float distanceToFindHero = 200;
    [SerializeField] float jumpForce = 400;


    [Header("--弾の撃ち方--")]
    [SerializeField] float secondsPerShoot = 0.3f;
    float secondsToShootNow = 0f;

    [SerializeField] int howManyShoots = 3;
    int howManyShootsNow = 0;

    [SerializeField] float upAngle = 10;
    [SerializeField] float downAngle = 70;
    [SerializeField] float num_tamaPerShoot = 5;

    [Header("--弾のパラメータ--")]
    [SerializeField] TamaController tama = null;
    [SerializeField] float tamaSpeedPerSec = 200;
    [SerializeField] float tamaLife = 1f;

    [Header("--子オブジェクト類--")]
    [SerializeField] GroundChecker groundChecker = null;
    [SerializeField] Transform gazosTF = null;
    [SerializeField] Transform baneTF = null;
    [SerializeField] Transform dodaiTF = null;
    [SerializeField] SpriteRenderer hontaiSR = null;
    [SerializeField] Sprite hontaiSpriteActive = null;
    [SerializeField] Sprite hontaiSpriteInactive = null;
    [SerializeField] Rigidbody2D RigidBody;

    [field: SerializeField, LabelText("DPCD")]
    public DPinEnemy DPCD{ get; private set; }

    static bool inNewScene; //なんかあほらしいな……
    static ObjectPool<TamaController> tamaPool;
    SoundGroup soundGroup;

    void Awake(){
        inNewScene = true;
    }

    void Start()
    {
        if(inNewScene){
            //これがどのタイミングで呼ばれるのか分からん(主人公が近づいてきてから呼ばれてたらつらい)
            tamaPool = new ObjectPool<TamaController>(tama, 128, DraftManager.CurrentInstance.GameMasterTF);
            inNewScene = false;
        }
        soundGroup = GetComponent<SoundGroup>();
    }

    void Update()
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
                if(RigidBody.velocity.y < 0){
                    state = State.Shoot;
                    secondsToShootNow = 0;
                    howManyShootsNow = howManyShoots;
                    baneTF.DOScaleY(0.3f, 0.5f);
                    dodaiTF.DOLocalMoveY(-23,0.5f);
                }
                break;
            
            case State.Shoot:
                RigidBody.simulated = false;
                secondsToShootNow -= TimeManager.Current.DeltaTimeExceptHero;

                if(secondsToShootNow <= 0)
                {
                    Shoot();
                    secondsToShootNow = secondsPerShoot;

                    howManyShootsNow --;
                    if(howManyShootsNow==0){
                        state = State.Rest;
                        RigidBody.simulated = true;
                    }
                }
                break;
            
            case State.Rest:
                restSecondsNow += TimeManager.Current.DeltaTimeExceptHero;

                if(restSecondsNow >= restSeconds){
                    restSecondsNow = 0;
                    if(MyMath.DistanceXY(transform.position, HeroDefiner.CurrentHeroPos) < distanceToFindHero){
                        Jump();
                    }
                    else{
                        state = State.Wait;
                        hontaiSR.sprite = hontaiSpriteInactive;
                    }

                }else if(groundChecker.IsOnGround) RigidBody.velocity = new Vector2();

                break;
        }
    }

    void Jump(){
        RigidBody.velocity = new Vector2(0,jumpForce);
        state = State.Jump;
        baneTF.DOScaleY(1, 0.3f).SetEase(Ease.InOutSine);
        dodaiTF.DOLocalMoveY(-46,0.3f).SetEase(Ease.InOutSine);
        soundGroup.Play("Jump");
    }

    void Shoot(){
        for(int i=0; i<num_tamaPerShoot; i++){

            float angle = - upAngle - i * (downAngle - upAngle) / (num_tamaPerShoot - 1);
            if(!EyeToRight) angle = - 180 - angle;
            TamaController imatama = tamaPool.ActivateOne(
                angle.ToString()
                + " " + tamaSpeedPerSec.ToString()
                + " " + tamaLife.ToString()
            );
            
            Vector3 offset = EyeToRight ? new Vector3(70,-30) : new Vector3(-70,-30);
            imatama.transform.position = transform.position + offset;
        }

        soundGroup.Play("Shoot");
    }
}
