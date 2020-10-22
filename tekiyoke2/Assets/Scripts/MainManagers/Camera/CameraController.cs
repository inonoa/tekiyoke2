using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;
using System;

public class CameraController : MonoBehaviour
{
    Camera cmr;
    private float defaultSize;
    [SerializeField] float zoomSizeMin = 300;
    [SerializeField] float zoomSpeed   = 100f;
    [SerializeField] float unzoomSpeed = 500f;

    [SerializeField] Vector2 fromCameraToHero = new Vector2(0, -100);

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

    enum StateAboutJet{ Default, ZoomingForDash, Dashing, Retreating }
    StateAboutJet jetState = StateAboutJet.Default;

    [SerializeField] float jetFreezeSeconds = 0.3f;

    //この辺必要か？？？
    public void StartZoomForDash() => jetState = StateAboutJet.ZoomingForDash;
    public void OnJet()
    {
        jetState = StateAboutJet.Dashing;
        Freeze(jetFreezeSeconds);
    }
    public void EndDash() => jetState = StateAboutJet.Retreating;
    public void Reset()   => jetState = StateAboutJet.Retreating; //多分要改善。ダッシュ以外のズームが導入された場合とか



    float seconds2freeze = 0;
    Vector3 freezePosition;

    public bool Freeze(float seconds = 0.3f)
    {
        if(seconds2freeze > 0) return false;

        seconds2freeze = seconds;
        freezePosition = transform.position;
        return true;
    }

    void OnEnable()  => gameObject.AddComponent<AudioListener>();
    void OnDisable() => Destroy(GetComponent<AudioListener>());

    void Start()
    {
        defaultSize = cmr.orthographicSize;
        targetPosition = HeroDefiner.CurrentHeroPos - fromCameraToHero.ToVec3() + new Vector3(0, 0, -500);
        scShoOutOfWindController.canvas = canvas;
    }

    void FixedUpdate()
    {
        switch(jetState)
        {
        case StateAboutJet.Default: break;

        case StateAboutJet.ZoomingForDash:

            if(cmr.orthographicSize > zoomSizeMin)
            {
                cmr.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime;
            }
            break;

        case StateAboutJet.Dashing: break;

        case StateAboutJet.Retreating:
            
            cmr.orthographicSize += unzoomSpeed * Time.unscaledDeltaTime;

            if(cmr.orthographicSize > defaultSize)
            {
                cmr.orthographicSize = defaultSize;
                jetState = StateAboutJet.Default;
            }
            break;
        }

        if(seconds2freeze > 0)
        {
            seconds2freeze -= Time.unscaledDeltaTime;
            return;
        }

        targetPosition = NextTartgetPosition(targetPosition);
        positionGap    = NextPositionGap(positionGap);

        transform.position = targetPosition.ToVec3() + positionGap.ToVec3() + new Vector3(0,0,-500);

        //単純に主人公の移動距離分追いかけたあと、Freeze中に置いてけぼりを喰らっていた分をちょっとずつ追い付く
        Vector2 NextTartgetPosition(Vector2 currentTargetPosition)
        {
            Vector2 lastPos   = HeroDefiner.CurrentHeroPastPos.Count > 1 ? HeroDefiner.CurrentHeroPastPos[1] : HeroDefiner.CurrentHeroPos;
            Vector2 dist      = MyMath.DistAsVector2(HeroDefiner.CurrentHeroPos, lastPos);
            Vector2 distAdded = currentTargetPosition + dist;

            Vector2 catchUp = (HeroDefiner.CurrentHeroExpectedPos - fromCameraToHero - targetPosition) * targetPosChangeSpeed;
            return distAdded + catchUp;
        }

        Vector2 NextPositionGap(Vector2 currentPositionGap)
        {
            float velocityMean      = HeroVelocityMean(100).x;
            float velocityThreshold = HeroDefiner.currentHero.Parameters.GroundSpeedMax * 0.3f;

            Vector2 targetGap;
            if     (velocityMean <  -velocityThreshold) targetGap = new Vector2(-positionGapWidth, 0);
            else if(velocityMean <=  velocityThreshold) targetGap = new Vector2( 0,                0);
            else                                        targetGap = new Vector2( positionGapWidth, 0);
            
            Vector2 current2target = targetGap - positionGap;
            if(current2target.magnitude < 1) return positionGap;
            return positionGap + current2target * positionGapChangeSpeed;
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
        cmr            = GetComponent<Camera>();
        AfterEffects   = GetComponent<AfterEffects>();
    }

    #endregion
}
