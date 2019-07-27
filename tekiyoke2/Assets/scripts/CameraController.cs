using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    enum StateOfCamera{
        Default, ZoomingForDash, Dashing, Retreating
    }

    StateOfCamera state = StateOfCamera.Default;

    bool toZoomRight = true;

    public Camera cmr;
    private float defaultSize;
    private Vector3 defaultPosition;

    public void StartZoomForDash(bool zoom2Right){
        state = StateOfCamera.ZoomingForDash;
        toZoomRight = zoom2Right;
    }
    public void Dash(){
        state = StateOfCamera.Dashing;
    }
    public void EndDash(){
        state = StateOfCamera.Retreating;
    }

    public void Reset(){
        //多分要改善。ダッシュ以外のズームが導入された場合とか
        state = StateOfCamera.Retreating;
    }



    // Start is called before the first frame update
    void Start()
    {
        cmr = GetComponent<Camera>();
        defaultSize = cmr.orthographicSize;
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state){
            case StateOfCamera.Default:
                break;

            case StateOfCamera.ZoomingForDash:
                if(cmr.orthographicSize>defaultSize/2){
                    cmr.orthographicSize -= 2;
                    if(toZoomRight){
                        transform.localPosition += new Vector3(1.5f,-0.4f,0);
                    }else{
                        transform.localPosition += new Vector3(-1.5f,-0.4f,0);
                    }
                }
                break;

            case StateOfCamera.Dashing:
                break;

            case StateOfCamera.Retreating:
                if(cmr.orthographicSize<defaultSize){
                    cmr.orthographicSize += 20;
                    if(toZoomRight){
                        transform.localPosition += new Vector3(-10,2.6f,0);
                    }else{
                        transform.localPosition += new Vector3(10,2.6f,0);
                    }

                    if(cmr.orthographicSize>defaultSize){
                        cmr.orthographicSize = defaultSize;
                        state = StateOfCamera.Default;
                        transform.localPosition = defaultPosition;
                    }
                }
                break;
        }
    }
}
