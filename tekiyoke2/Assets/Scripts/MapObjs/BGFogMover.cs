using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFogMover : MonoBehaviour
{
    [SerializeField] Vector3 speed = new Vector3(-0.1f,0,0);
    [SerializeField] [Range(0, 1)] float depth = 0.3f;

    ///<summary>カメラはFixedUpdate()内で移動しておりカメラに記録しとくよりこっちの方がいい……？(カメラのUpdate()内に書けばいいのか)</summary>
    Vector3 lastCameraPos;

    Vector3 defPos;
    Vector2 screenEdge = new Vector2(2000, 1200);

    void Start()
    {
        lastCameraPos = CameraController.CurrentCameraPos;
        defPos = transform.localPosition;
        GetComponent<SpriteRenderer>().material.SetFloat("_AlphaRate", 0.2f + 0.8f * depth);
    }

    void Update()
    {
        Vector3 cameraMove = CameraController.CurrentCameraPos - lastCameraPos;
        lastCameraPos = CameraController.CurrentCameraPos;

        transform.localPosition += - cameraMove * (1 - depth) + speed;

        if(transform.localPosition.x >  screenEdge.x ) transform.localPosition += new Vector3(-2*screenEdge.x,               0, 0);
        if(transform.localPosition.x < -screenEdge.x)  transform.localPosition += new Vector3( 2*screenEdge.x,               0, 0);
        if(transform.localPosition.y >  screenEdge.y)  transform.localPosition += new Vector3(              0, -2*screenEdge.y, 0);
        if(transform.localPosition.y < -screenEdge.y)  transform.localPosition += new Vector3(              0,  2*screenEdge.y, 0);
    }
}
