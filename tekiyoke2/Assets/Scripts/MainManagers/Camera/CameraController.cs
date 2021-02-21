using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;
using System;
using Sirenix.OdinInspector;

public class CameraController : MonoBehaviour
{
    CameraZoomerForJet zoomer;

    [SerializeField] Vector2 fromCameraToHero = new Vector2(0, -100);

    ///<summary>主人公を追いかけている、主人公が動くと遅れてついていく</summary>
    Vector2 targetPosition;

    ///<summary>Freeze後に追いかけるスピードとかに比例してる</summary>
    [SerializeField] float targetPosChangeSpeed = 0.1f;

    ///<summary>右に移動中は右に、左に移動中は左に寄る、みたいなのの度合い</summary>
    Vector2 positionGap = Vector2.zero;

    ///<summary>動いている向き(左右のみ)にこの幅だけカメラが先行して(？)動く</summary>
    [SerializeField] float positionGapWidth = 120;

    ///<summary>positionGapが変化するスピード</summary>
    [SerializeField] float positionGapChangeSpeed = 0.1f;

    [SerializeField] float jetFreezeSeconds = 0.3f;

    [Space(10)]
    [SerializeField] ScShoOutOfWindController scShoOutOfWindController;
    [SerializeField] ScShoController scShoController;
    public AfterEffects AfterEffects{ get; private set; }

    [SerializeField] Canvas canvas;

    public void StartZoomForJet() => zoomer.StartZoom();
    public void OnJet()
    {
        zoomer.OnJet();
        Freeze(jetFreezeSeconds);
    }
    public void EndJet() => zoomer.EndJet();
    public void Reset() => zoomer.Reset_();



    float seconds2freeze = 0;
    public bool Freeze(float seconds = 0.3f)
    {
        if(seconds2freeze > 0) return false;

        seconds2freeze = seconds;
        return true;
    }

    void Start()
    {
        targetPosition = HeroDefiner.CurrentPos - fromCameraToHero.ToVec3() + new Vector3(0, 0, -500);
        scShoOutOfWindController.canvas = canvas;
    }

    void FixedUpdate()
    {
        if(seconds2freeze > 0)
        {
            seconds2freeze -= Time.unscaledDeltaTime;
            return;
        }

        targetPosition = NextTartgetPosition(targetPosition);
        positionGap    = NextPositionGap(positionGap);

        transform.position = targetPosition.ToVec3() + positionGap.ToVec3() + new Vector3(0, 0, -500);
    }

    //単純に主人公の移動距離分追いかけたあと、Freeze中に置いてけぼりを喰らっていた分をちょっとずつ追い付く
    Vector2 NextTartgetPosition(Vector2 currentTargetPosition)
    {
        Vector2 heroPosLastToCurrent = HeroPosLastToCurrent();
        Vector2 dist = new Vector2
        (
            XLocked ? 0 : heroPosLastToCurrent.x,
            YLocked ? 0 : heroPosLastToCurrent.y
        );
        Vector2 distAdded = currentTargetPosition + dist;

        Vector2 catchUp = (FinalTargetPos() - targetPosition) * targetPosChangeSpeed;
        return distAdded + catchUp;
    }

    CameraLockingArea LockedBy => HeroDefiner.currentHero.DetectsCameraLockingArea.LockedBy;

    bool XLocked => !(LockedBy is null) && LockedBy.LockX;
    bool YLocked => !(LockedBy is null) && LockedBy.LockY;

    Vector2 HeroPosLastToCurrent()
    {
        Vector2 lastPos   = HeroDefiner.PastPoss.Count > 1 ? HeroDefiner.PastPoss[1] : HeroDefiner.CurrentPos;
        return MyMath.DistAsVector2(HeroDefiner.CurrentPos, lastPos);
    }

    Vector2 FinalTargetPos()
    {
        if (LockedBy is null)
        {
            return HeroDefiner.ExpectedPos - fromCameraToHero;
        }
        else
        {
            Vector2 fromHero = HeroDefiner.ExpectedPos - fromCameraToHero;
            Vector2 fromLock = LockedBy.transform.position;
            return new Vector2
            (
                XLocked ? fromLock.x : fromHero.x,
                YLocked ? fromLock.y : fromHero.y
            );
        }
    }

    Vector2 NextPositionGap(Vector2 currentPositionGap)
    {
        Vector2 targetGap = XLocked ? Vector2.zero : NextTargetGapNormal();
            
        Vector2 current2target = targetGap - currentPositionGap;
        if(current2target.magnitude < 1) return currentPositionGap;
        return currentPositionGap + current2target * positionGapChangeSpeed;
    }

    Vector2 NextTargetGapNormal()
    {
        bool wantsToGoRight = HeroDefiner.currentHero.WantsToGoRight;
        return wantsToGoRight ? new Vector2(positionGapWidth, 0) : new Vector2(-positionGapWidth, 0);
    }

    public void ScSho(Action<Texture2D> callbackOnTaken)
        => scShoController.BeginScSho(callbackOnTaken);

    public void ScShoOutOfWind(Action<Texture2D> callbackOnTaken)
        => scShoOutOfWindController.BeginScShoOutOfWind(callbackOnTaken);
    
    
    void OnEnable()  => gameObject.AddComponent<AudioListener>();
    void OnDisable() => Destroy(GetComponent<AudioListener>());
    

    #region instance

    public static CameraController Current { get; private set; }
    public static Vector3 CurrentCameraPos => Current.transform.position;

    void Awake()
    {
        Current = this;
        AfterEffects   = GetComponent<AfterEffects>();
        zoomer         = GetComponent<CameraZoomerForJet>();
    }

    void OnDestroy()
    {
        Current = null;
    }

    #endregion
}
