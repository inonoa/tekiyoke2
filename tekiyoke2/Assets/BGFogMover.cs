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

    void Start()
    {
        lastCameraPos = CameraController.CurrentCameraPos;
        defPos = transform.localPosition;
        Color tmp = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(tmp.r, tmp.g, tmp.b, (0.8f - 0.5f * depth));
    }

    void Update()
    {
        Vector3 cameraMove = CameraController.CurrentCameraPos - lastCameraPos;
        lastCameraPos = CameraController.CurrentCameraPos;

        transform.localPosition += - cameraMove * (1 - depth) + speed;

        if(transform.localPosition.x > 1000 ) transform.localPosition += new Vector3(-2000,     0, 0);
        if(transform.localPosition.x < -1000) transform.localPosition += new Vector3( 2000,     0, 0);
        if(transform.localPosition.y > 750)   transform.localPosition += new Vector3(    0, -1500, 0);
        if(transform.localPosition.y < -750)  transform.localPosition += new Vector3( 1500,  1500, 0);
    }
}
