using System.Collections;
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

    Vector2 targetPosition;

    ///<summary>Freeze後に追いかけるスピードとかに比例してる</summary>
    static readonly float targetPosChangeSpeed = 0.1f;

    Vector2 positionGap = Vector2.zero;

    ///<summary>動いている向き(左右のみ)にこの幅だけカメラが先行して(？)動く</summary>
    static readonly float positionGapWidth = 120;

    ///<summary>positionGapが変化するスピード</summary>
    static readonly float positionGapChangeSpeed = 0.1f;

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
            targetPosition += MyMath.DistAsVector2(HeroDefiner.CurrentHeroExpectedPos, HeroDefiner.CurrentHeroPastPos[0]);
            targetPosition += (HeroDefiner.CurrentHeroExpectedPos + new Vector2(0,100) - targetPosition) * targetPosChangeSpeed;

            //Update positionGap
            Vector2 heroVec = new Vector2(MyMath.FloorAndCeil(-1,HeroVelocityMean(3).x,1),0);
            Vector2 dist2Gap = heroVec * positionGapWidth - positionGap;
            if(dist2Gap.magnitude < 1) positionGap += Vector2.zero;
            else                       positionGap += (heroVec * positionGapWidth - positionGap) * positionGapChangeSpeed;


            transform.position = targetPosition.ToVector3() + positionGap.ToVector3() + new Vector3(0,0,-200);
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

    #region instance

    static CameraController _CurrentCamera;
    public static CameraController CurrentCamera{ get => _CurrentCamera; }
    public static Vector3 CurrentCameraPos{ get => _CurrentCamera.transform.position; }

    void Awake() => _CurrentCamera = this;

    #endregion
}
