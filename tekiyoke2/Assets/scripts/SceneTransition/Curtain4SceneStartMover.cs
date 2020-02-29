using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain4SceneStartMover : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(50,0);
        if(gameObject.transform.localPosition.x>4000) Destroy(gameObject);
    }
}
