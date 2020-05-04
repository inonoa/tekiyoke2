using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>不要じゃね？</summary>
public class Title2Stages : MonoBehaviour
{
    public CloudSpawner clouds;
    
    void Update()
    {

        if(InputManager.Instance.AnyButtonDown()){
            if(clouds.state==CloudSpawner.State.Active){
                clouds.state = CloudSpawner.State.Wind;
                GetComponent<SoundGroup>().Play("Push");
            }
        }
    }
}
