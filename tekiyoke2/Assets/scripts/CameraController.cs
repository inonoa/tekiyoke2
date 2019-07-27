using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public void SetPos(float localX, float localY){
        transform.localPosition = new Vector3(localX,localY,-100);
    }

    public void ZoomIn(){
        
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
