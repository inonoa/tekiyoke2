using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain4SceneStartMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(50,0);
        if(gameObject.transform.localPosition.x>4000) Destroy(gameObject);
    }
}
