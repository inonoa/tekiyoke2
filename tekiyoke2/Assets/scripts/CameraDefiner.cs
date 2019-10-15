using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefiner : MonoBehaviour
{

    public static CameraController CurrentCamera{ get; set; }
    public static Vector3 CurrentCameraPos{
        get {return CurrentCamera.transform.position; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
