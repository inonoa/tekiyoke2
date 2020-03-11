using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMover : MonoBehaviour
{

    public float bgrate = 0.1f;

    [SerializeField] Vector3 zure = new Vector2();
    
    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = new Vector3(CameraController.CurrentCameraPos.x/(1+bgrate),CameraController.CurrentCameraPos.y/(1+bgrate))
                                  + zure + new Vector3(0,50);
    }
}
