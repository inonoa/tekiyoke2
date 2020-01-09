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

    [SerializeField]
    float approachV;

    (float x, float y) velocity = (0,0);
    float dvMax = 20;

    enum CameraStateAboutDash{ Default, ZoomingForDash, Dashing, Retreating }
    CameraStateAboutDash dashState = CameraStateAboutDash.Default;

    //この辺必要か？？？
    public void StartZoomForDash() => dashState = CameraStateAboutDash.ZoomingForDash;
    public void Dash() => dashState = CameraStateAboutDash.Dashing;
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
                        transform.position = HeroDefiner.CurrentHeroPos + new Vector3(0,-50,-200); //tmp
                    }
                }
                break;
        }

        if(frames2freeze>0){
            //transform.position = freezePosition;
            frames2freeze --;
        }else{
            Vector2 targetPos = (Vector2)HeroDefiner.CurrentHeroPos
                                + 5 * new Vector2(HeroDefiner.currentHero.velocity.x * 2, HeroDefiner.currentHero.velocity.y);

            float distance_x = targetPos.x - transform.position.x;
            float distance_y = targetPos.y - (transform.position.y - 100);
            int distance_x_sign = (distance_x>0) ? 1 : -1;
            int distance_y_sign = (distance_y>0) ? 1 : -1;

            float velX = distance_x*distance_x * distance_x_sign * approachV;
            float velY = distance_y*distance_y * distance_y_sign * approachV;

            float dvX = velX - velocity.x;
            float dvY = velY - velocity.y;

            velocity.x += dvX>0 ? Min(dvX, dvMax) : - Min(-dvX, dvMax);
            velocity.y += dvY>0 ? Min(dvY, dvMax) : - Min(-dvY, dvMax);

            transform.position += new Vector3(velocity.x, velocity.y);
        }
    }

    #region instance

    static CameraController _CurrentCamera;
    public static CameraController CurrentCamera{ get => _CurrentCamera; }
    public static Vector3 CurrentCameraPos{ get => _CurrentCamera.transform.position; }

    void Awake() => _CurrentCamera = this;

    #endregion
}
