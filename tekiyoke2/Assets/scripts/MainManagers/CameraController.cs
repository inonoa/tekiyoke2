using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cmr;
    private float defaultSize;
    private Vector3 defaultLocalPosition;
    public float approachV;


    bool toZoomRight = true;

    enum CameraStateAboutDash{ Default, ZoomingForDash, Dashing, Retreating }
    CameraStateAboutDash dashState = CameraStateAboutDash.Default;

    //この辺必要か？？？
    public void StartZoomForDash(bool zoom2Right)
                    { dashState = CameraStateAboutDash.ZoomingForDash; toZoomRight = zoom2Right; }
    public void Dash(){ dashState = CameraStateAboutDash.Dashing; }
    public void EndDash(){ dashState = CameraStateAboutDash.Retreating; }
    public void Reset(){ dashState = CameraStateAboutDash.Retreating; } //多分要改善。ダッシュ以外のズームが導入された場合とか



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
        defaultLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        switch(dashState){
            case CameraStateAboutDash.Default: break;

            case CameraStateAboutDash.ZoomingForDash:
                if(cmr.orthographicSize>defaultSize/2){
                    cmr.orthographicSize -= 2;
                    transform.localPosition += (toZoomRight ? new Vector3(1.5f,-0.4f,0) : new Vector3(-1.5f,-0.4f,0));
                }
                break;

            case CameraStateAboutDash.Dashing: break;

            case CameraStateAboutDash.Retreating:
                if(cmr.orthographicSize<defaultSize){
                    cmr.orthographicSize += 20;
                    transform.localPosition += (toZoomRight ? new Vector3(-10,2.6f,0) : new Vector3(10,2.6f,0));

                    if(cmr.orthographicSize>defaultSize){
                        cmr.orthographicSize = defaultSize;
                        dashState = CameraStateAboutDash.Default;
                        transform.localPosition = defaultLocalPosition;
                    }
                }
                break;
        }

        if(frames2freeze>0){
            transform.position = freezePosition;
            frames2freeze --;
        }else{
            float distance_x = HeroDefiner.CurrentHeroPos.x - transform.position.x;
            float distance_y = HeroDefiner.CurrentHeroPos.y - (transform.position.y - 100);
            int distance_x_sign = (distance_x>0) ? 1 : -1;
            int distance_y_sign = (distance_y>0) ? 1 : -1;

            transform.position += new Vector3(distance_x*distance_x * distance_x_sign * approachV, 
                                              distance_y*distance_y * distance_y_sign * approachV );
        }
    }

    #region instance

    static CameraController _CurrentCamera;
    public static CameraController CurrentCamera{ get => _CurrentCamera; }
    public static Vector3 CurrentCameraPos{ get => _CurrentCamera.transform.position; }

    void Awake() => _CurrentCamera = this;

    #endregion
}
