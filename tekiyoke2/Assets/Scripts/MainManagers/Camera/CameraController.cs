using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;
using System;

public class CameraController : MonoBehaviour
{
    Camera cmr;
    private float defaultSize;
    static readonly float zoomSizeMin = 300;
    static readonly float zoomSpeed = 1;
    static readonly float unzoomSpeed = 10;

    [SerializeField] ScShoOutOfWindController scShoOutOfWindController;
    [SerializeField] ScShoController scShoController;
    public AfterEffects AfterEffects{ get; private set; }

    [SerializeField] Canvas canvas;

    ///<summary>主人公を追いかけている、主人公が動くと遅れてついていく</summary>
    Vector2 targetPosition;

    ///<summary>Freeze後に追いかけるスピードとかに比例してる</summary>
    static readonly float targetPosChangeSpeed = 0.1f;

    ///<summary>右に移動中は右に、左に移動中は左に寄る、みたいなのの度合い</summary>
    Vector2 positionGap = Vector2.zero;

    ///<summary>動いている向き(左右のみ)にこの幅だけカメラが先行して(？)動く</summary>
    static readonly float positionGapWidth = 120;

    ///<summary>positionGapが変化するスピード</summary>
    static readonly float positionGapChangeSpeed = 0.1f;

    enum CameraStateAboutDash{ Default, ZoomingForDash, Dashing, Retreating }
    CameraStateAboutDash dashState = CameraStateAboutDash.Default;

    [SerializeField] int jetFreezeFrames = 20;

    //この辺必要か？？？
    public void StartZoomForDash() => dashState = CameraStateAboutDash.ZoomingForDash;
    public void OnJet()
    {
        dashState = CameraStateAboutDash.Dashing;
        Freeze(jetFreezeFrames);
    }
    public void EndDash() => dashState = CameraStateAboutDash.Retreating;
    public void Reset() => dashState = CameraStateAboutDash.Retreating; //多分要改善。ダッシュ以外のズームが導入された場合とか



    int frames2freeze = 0;
    Vector3 freezePosition;

    public bool Freeze(int num_frames = 20){
        if(frames2freeze > 0) return false;

        frames2freeze = num_frames;
        freezePosition = transform.position;
        return true;
    }

    void OnEnable() => gameObject.AddComponent<AudioListener>();
    void OnDisable() => Destroy(GetComponent<AudioListener>());

    void Start()
    {
        defaultSize = cmr.orthographicSize;
        targetPosition = HeroDefiner.CurrentHeroPos + new Vector3(0,100,-500);
        scShoOutOfWindController.canvas = canvas;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.P)) ScShoOutOfWind(ss => print("SCSHO"));
    }

    void FixedUpdate()
    {
        switch(dashState){
            case CameraStateAboutDash.Default: break;

            case CameraStateAboutDash.ZoomingForDash:
                if(cmr.orthographicSize>zoomSizeMin){
                    cmr.orthographicSize -= zoomSpeed;
                }
                break;

            case CameraStateAboutDash.Dashing: break;

            case CameraStateAboutDash.Retreating:
                if(cmr.orthographicSize<defaultSize){
                    cmr.orthographicSize += unzoomSpeed;

                    if(cmr.orthographicSize>defaultSize){
                        cmr.orthographicSize = defaultSize;
                        dashState = CameraStateAboutDash.Default;
                    }
                }
                break;
        }

        if(frames2freeze>0){
            frames2freeze --;
        }else{
            //Update targetPosition
            //単純に主人公の移動距離分追いかけたあと、Freeze中に置いてけぼりを喰らっていた分をちょっとずつ追い付く
            targetPosition += MyMath.DistAsVector2(HeroDefiner.CurrentHeroPos,
                HeroDefiner.CurrentHeroPastPos.Count > 1 ? HeroDefiner.CurrentHeroPastPos[1] : HeroDefiner.CurrentHeroPos);
            targetPosition += (HeroDefiner.CurrentHeroExpectedPos + new Vector2(0,100) - targetPosition) * targetPosChangeSpeed;

            //Update positionGap
            float velMX = HeroVelocityMean(100).x;
            float zureX;
            if     (velMX < - HeroDefiner.currentHero.Parameters.GroundSpeedMax * 0.6f) zureX = -1;
            else if(velMX <   HeroDefiner.currentHero.Parameters.GroundSpeedMax * 0.6f) zureX = 0;
            else                                                                        zureX = 1;
            Vector2 dist2Gap = new Vector2(zureX,0) * positionGapWidth - positionGap;
            if(dist2Gap.magnitude < 1) positionGap += Vector2.zero;
            else                       positionGap += dist2Gap * positionGapChangeSpeed;


            transform.position = targetPosition.ToVec3() + positionGap.ToVec3() + new Vector3(0,0,-500);
        }
    }

    ///<summary>velocityというか実際に移動した距離の平均</summary>
    Vector2 HeroVelocityMean(int range){
        int count = HeroDefiner.CurrentHeroPastPos.Count;
        if(count >= range)
            return (MyMath.DistAsVector2(HeroDefiner.CurrentHeroExpectedPos, HeroDefiner.CurrentHeroPastPos[range - 1])) / range;
        else
            return (MyMath.DistAsVector2(HeroDefiner.CurrentHeroExpectedPos, HeroDefiner.CurrentHeroPastPos[count - 1])) / count;
    }

    public void ScSho(Action<Texture2D> callbackOnTaken)
        => scShoController.BeginScSho(callbackOnTaken);

    public void ScShoOutOfWind(Action<Texture2D> callbackOnTaken)
        => scShoOutOfWindController.BeginScShoOutOfWind(callbackOnTaken);
    

    #region instance

    static CameraController _CurrentCamera;
    public static CameraController CurrentCamera{ get => _CurrentCamera; }
    public static Vector3 CurrentCameraPos{ get => _CurrentCamera.transform.position; }

    void Awake()
    {
        _CurrentCamera = this;
        cmr = GetComponent<Camera>();
        AfterEffects = GetComponent<AfterEffects>();
    }

    #endregion
}
