using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMover : MonoBehaviour
{

    public float bgrate = 0.1f;
    
    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = new Vector2(CameraController.CurrentCameraPos.x/(1+bgrate),CameraController.CurrentCameraPos.y/(1+bgrate)+50);
    }
}
