using System;
using UnityEngine;

public class CameraZoomerForJet : MonoBehaviour
{
    [SerializeField] float zoomSizeMin = 350;
    [SerializeField] float zoomSpeed   = 100f;
    [SerializeField] float unzoomSpeed = 500f;
    
    float defaultSize;
    new Camera camera;
    
    enum EState{ Default, ZoomingForDash, Dashing, Retreating }
    EState jetState = EState.Default;
    
    //この辺必要か？？？
    public void StartZoom() => jetState = EState.ZoomingForDash;
    public void OnJet()     => jetState = EState.Dashing;
    public void EndJet()    => jetState = EState.Retreating;
    public void Reset_()    => jetState = EState.Retreating;
    
    void FixedUpdate()
    {
        switch(jetState)
        {
            case EState.Default: break;

            case EState.ZoomingForDash:

                if(camera.orthographicSize > zoomSizeMin)
                {
                    camera.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime;
                }
                break;

            case EState.Dashing: break;

            case EState.Retreating:
            
                camera.orthographicSize += unzoomSpeed * Time.unscaledDeltaTime;

                if(camera.orthographicSize > defaultSize)
                {
                    camera.orthographicSize = defaultSize;
                    jetState = EState.Default;
                }
                break;
        }
    }

    void Start()
    {
        camera = GetComponent<Camera>();
        defaultSize = camera.orthographicSize;
    }
}