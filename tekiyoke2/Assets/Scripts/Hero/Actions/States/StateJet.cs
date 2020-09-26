using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StateJet : HeroState
{
    static readonly float timeScaleBeforeJet = 0.2f;

    HeroMover hero;
    Vector3 posWhenJet;
    GameObject jetStream;
    BoxCollider2D jsCol;
    Transform trailTF;
    Collider2D col2DP;

    JetCloudManager clouds;
    PostEffectWrapper vignette;
    Tween vignetteTween;
    PostEffectWrapper blurY;
    Tween blurYTween;
    PostEffectWrapper blurT;
    Tween blurTTween;

    enum State { Ready, Jetting }
    State state = State.Ready;
    bool jet2Right = true;

    int tameFrames = 0;
    int jetFrames = 0;
    int jetFramesMax;

    float[] jetVelocities;

    public StateJet(HeroMover hero){
        this.hero = hero;
        clouds = GameUIManager.CurrentInstance.JetCloud;
        vignette = CameraController.CurrentCamera.AfterEffects.Find("Vignette");
        blurY = CameraController.CurrentCamera.AfterEffects.Find("BlurEdge1");
        blurT = CameraController.CurrentCamera.AfterEffects.Find("BlurEdge2");
    }
    public override void Try2StartJet(){ }
    public override void Try2EndJet(){
        if(state==State.Ready){
            hero.CanBeDamaged = false;
            state = State.Jetting;
            PhantomAndDissolve();
            hero.Anim.SetTrigger(jet2Right ? "runr" : "runl");
            Tokitome.SetTime(1);
            hero.SoundGroup.Play("Jet");
            hero.SoundGroup.Stop("Tame");
            //hero.spriteRenderer.color = new Color(1,1,1,0.3f);

            //Jetする距離を計算
            int fullDist = (int)(MyMath.FloorAndCeil(10,tameFrames,30) * 25);
            jetFramesMax = (fullDist*3) /100;
            jetVelocities = new float[jetFramesMax];
            for(int i=0;i<jetFramesMax;i++){
                jetVelocities[i] = fullDist * ( EasingFunc((i+1)/(float)jetFramesMax) - EasingFunc(i/(float)jetFramesMax) );
            }
            posWhenJet = hero.transform.position;
            // jetStream = GameObject.Instantiate(hero.ObjsHolderForStates.JetstreamPrefab, DraftManager.CurrentInstance.GameMasterTF);
            // jetStream.transform.position = hero.transform.position;
            jsCol = jetStream.GetComponent<BoxCollider2D>();

            col2DP = hero.GetDPinEnemy.GetComponent<Collider2D>();
            col2DP.enabled = true;

            //風エフェクト
            //trailTF = GameObject.Instantiate(hero.ObjsHolderForStates.JetTrail, DraftManager.CurrentInstance.GameMasterTF).transform;
            trailTF.position = hero.transform.position;
            trailTF.GetComponent<TrailRenderer>().time = jetFramesMax / 60f;

            clouds.EndClouds();

            //この辺どうにかならんか……

            vignetteTween.Kill();
            Sequence endSeq_tmp = DOTween.Sequence();
            endSeq_tmp.Append(DOTween.To(vignette.GetVolume, vignette.SetVolume, -0.3f - vignette.GetVolume() / 2, 0.2f).SetEase(Ease.OutSine));
            endSeq_tmp.Append(DOTween.To(vignette.GetVolume, vignette.SetVolume, 0, 0.3f).SetEase(Ease.InOutSine));
            endSeq_tmp.onComplete += () => vignette.IsActive = false;
            vignetteTween = endSeq_tmp;

            blurYTween.Kill();
            blurYTween = DOTween.To(blurY.GetVolume, blurY.SetVolume, 0, 0.1f);
            blurYTween.onComplete += () => blurY.IsActive = false;

            blurTTween.Kill();
            blurTTween = DOTween.To(blurT.GetVolume, blurT.SetVolume, 0, 0.1f);
            blurTTween.onComplete += () => blurT.IsActive = false;
            
            //hero.CmrCntr.OnJet(jetFramesMax); //今は何も起こってなさそう
        }
    }

    void PhantomAndDissolve(){
        SpriteRenderer phantom = hero.ObjsHolderForStates.PhantomRenderer;
        phantom.gameObject.SetActive(true);
        phantom.transform.position = hero.transform.position;
        phantom.sprite = hero.SpriteRenderer.sprite;

        Material heroMat = hero.SpriteRenderer.material;
        Material phantomMat = phantom.material;
        heroMat.SetFloat("_DisThreshold0", 1);
        heroMat.SetFloat("_DisThreshold1", 1.1f);
        phantomMat.SetFloat("_DisThreshold0", -1);
        phantomMat.SetFloat("_DisThreshold1", 0);

        float heroAppearSec = 0.3f;
        DOVirtual.DelayedCall(0.2f, () => {
            DOTween.To(
                () => heroMat.GetFloat("_DisThreshold0"),
                t0 => heroMat.SetFloat("_DisThreshold0", t0),
                -1,
                heroAppearSec
            );
            DOTween.To(
                () => heroMat.GetFloat("_DisThreshold1"),
                t1 => heroMat.SetFloat("_DisThreshold1", t1),
                0,
                heroAppearSec
            );
        });

        float phantomDisappearSec = 0.3f;
        DOTween.To(
            () => phantomMat.GetFloat("_DisThreshold0"),
            t0 => phantomMat.SetFloat("_DisThreshold0", t0),
            1,
            phantomDisappearSec
        );
        DOTween.To(
            () => phantomMat.GetFloat("_DisThreshold1"),
            t1 => phantomMat.SetFloat("_DisThreshold1", t1),
            1.1f,
            phantomDisappearSec
        ).onComplete = () => phantom.gameObject.SetActive(false);
    }

    public override void Try2Jump(){ }
    public override void Try2StartMove(bool toRight){
        if(state==State.Ready) jet2Right = toRight;
    }
    public override void Try2EndMove(){ }
    public override void Start(){
        Tokitome.SetTime(timeScaleBeforeJet);

        switch(hero.KeyDirection){
            case 1:
                jet2Right = true;
                break;
            case -1:
                jet2Right = false;
                break;
            case 0:
                jet2Right = hero.WantsToGoRight;
                break;
        }

        hero.CmrCntr.StartZoomForDash();
        clouds.StartClouds();
        hero.SoundGroup.SetVolume("Tame", 0);
        hero.SoundGroup.Play("Tame");
        hero.SoundGroup.VolumeTo("Tame", 1, 0.7f);

        vignette.IsActive = true;
        vignette.SetVolume(0);
        vignetteTween?.Kill();
        vignetteTween = DOTween.To(vignette.GetVolume, vignette.SetVolume, 2, 0.6f);

        blurY.IsActive = true;
        blurY.SetVolume(0);
        blurYTween?.Kill();
        blurYTween = DOTween.To(blurY.GetVolume, blurY.SetVolume, 2, 0.6f);

        blurT.IsActive = true;
        blurT.SetVolume(0);
        blurTTween?.Kill();
        blurTTween = DOTween.To(blurT.GetVolume, blurT.SetVolume, 2, 0.6f);
    }

    public override void Resume(){
        if(state==State.Jetting) hero.Anim.SetTrigger(jet2Right ? "runr" : "runl");
        // else書きたいがどうしよね、そもそもJetの遷移のタイミング変じゃないか？
    }

    public override void Update(){
        switch(state){

            case State.Ready:
                //ためすぎるとエンスト？してダメージ受けるとかしたいね
                tameFrames ++;
                if(!hero.IsOnGround) hero.velocity.Y -= HeroMover.gravity;
                break;

            case State.Jetting:
                hero.velocity = new HeroVelocity(jet2Right ? jetVelocities[jetFrames] : -jetVelocities[jetFrames] , 0);

                jetStream.transform.position = (posWhenJet + hero.transform.position) / 2;
                float colWidth  = Mathf.Abs(hero.transform.position.x - posWhenJet.x) + 20;
                float colHeight = Mathf.Abs(hero.transform.position.y - posWhenJet.y) + 80;
                jsCol.size = new Vector2(colWidth, colHeight);
                trailTF.position = hero.transform.position;

                jetFrames ++;
                if(jetFrames == jetFramesMax){

                    if(hero.IsOnGround){
                        hero.States.Push(new StateWait(hero));

                    }else{
                        // 空中にいる場合に二段目のジャンプができるかどうかが以前の状態に依存しているので聞きに行く
                        bool canJump = true;

                        HeroState tmp = hero.States.Pop(); //これは自分自身
                        if(hero.States.Peek() is IAskCanJump state2ask){
                            canJump = state2ask.CanJump;
                        }
                        hero.States.Push(tmp);

                        hero.States.Push(new StateFall(hero, canJump));
                    }
                }
                break;
        }
    }

    static float EasingFunc(float t){
        if(t>1) Debug.Log("t>1になっています");
        if(t<0) Debug.Log("t<0になっています");
        
        float zero2one = MyMath.FloorAndCeil(0,t,1);

        if(zero2one<0.2f)
            return 4 * zero2one;
        else
            return 0.8f + 0.25f * (zero2one - 0.2f);
    }

    public override void Exit(){
        hero.CanBeDamaged = true;
        //hero.spriteRenderer.color = new Color(1,1,1,1);
        hero.CmrCntr.EndDash();
        GameObject.Destroy(jetStream);
        if(col2DP!=null) col2DP.enabled = false;
        clouds.EndClouds();
        hero.SoundGroup.Stop("Tame");
        foreach(PostEffectWrapper pe in new PostEffectWrapper[]{vignette, blurT, blurY}){
            pe.IsActive = false;
        }
    }
}
