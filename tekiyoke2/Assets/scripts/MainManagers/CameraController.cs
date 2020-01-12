﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class CameraController : MonoBehaviour
{
    public Camera cmr;
    private float defaultSize;
    static readonly float zoomSizeMin = 300;
    static readonly float zoomSpeed = 1;
    static readonly float unzoomSpeed = 10;

    [SerializeField]
    float approachV;

    Vector2 targetPosition;
    Vector2 positionGap = Vector2.zero;
    static readonly float positionGapRadiusY = 100;

    enum CameraStateAboutDash{ Default, ZoomingForDash, Dashing, Retreating }
    CameraStateAboutDash dashState = CameraStateAboutDash.Default;

    //この辺必要か？？？
    public void StartZoomForDash() => dashState = CameraStateAboutDash.ZoomingForDash;
    public void Dash(int jetFrames){
        dashState = CameraStateAboutDash.Dashing;
        Freeze(jetFrames);
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



    // Start is called before the first frame update
    void Start()
    {
        cmr = GetComponent<Camera>();
        defaultSize = cmr.orthographicSize;
        targetPosition = HeroDefiner.CurrentHeroPos + new Vector3(0,100,-200);
    }

    // Update is called once per frame
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
            if(HeroDefiner.CurrentHeroPastPos.Count > 1)
                targetPosition += (HeroDefiner.CurrentHeroPos - HeroDefiner.CurrentHeroPastPos[1]).ToVector2();
            targetPosition += (HeroDefiner.CurrentHeroPos.ToVector2() + new Vector2(0,100) - targetPosition) / 10;

            //Update positionGap
            Vector2 heroVec = HeroVelocityMean(15);
            //Normalizeとはいったものの楕円との交点を取ってる
            Vector2 heroVecNormalized;
            if(heroVec.magnitude < 1) heroVecNormalized = Vector2.zero;
            else                      heroVecNormalized = heroVec / (float)System.Math.Sqrt(heroVec.x*heroVec.x /4 + heroVec.y*heroVec.y);

            Vector2 dist2Gap = heroVecNormalized * positionGapRadiusY - positionGap;
            if(dist2Gap.magnitude < 1) positionGap += Vector2.zero;
            else                       positionGap += (heroVecNormalized * positionGapRadiusY - positionGap) / 50;


            transform.position = targetPosition.ToVector3() + positionGap.ToVector3() + new Vector3(0,0,-200);
        }
    }

    ///<summary>velocityというか実際に移動した距離の平均</summary>
    Vector2 HeroVelocityMean(int range){

        int count = HeroDefiner.CurrentHeroPastPos.Count;
        if(count > range)
            return (HeroDefiner.CurrentHeroPos - HeroDefiner.CurrentHeroPastPos[range]) / range;
        else
            return (HeroDefiner.CurrentHeroPos - HeroDefiner.CurrentHeroPastPos[count-1]) / range;
    }

    #region instance

    static CameraController _CurrentCamera;
    public static CameraController CurrentCamera{ get => _CurrentCamera; }
    public static Vector3 CurrentCameraPos{ get => _CurrentCamera.transform.position; }

    void Awake() => _CurrentCamera = this;

    #endregion
}
