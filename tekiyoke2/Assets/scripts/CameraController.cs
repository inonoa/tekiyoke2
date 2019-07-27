using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cmr;
    private float defaultSize;

    public void SetPos(float localX, float localY){
        transform.localPosition = new Vector3(localX,localY,-100);
    }

    public void ZoomIn(float rate){
        cmr.orthographicSize = defaultSize / rate;
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultSize = cmr.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
