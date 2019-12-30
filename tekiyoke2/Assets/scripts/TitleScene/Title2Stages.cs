using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title2Stages : MonoBehaviour
{
    public CloudSpawner clouds;
    public GameObject pressaAnyKey;

    bool buttonPushed = false;

    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!buttonPushed){
            count ++;
            if(count==40){
                pressaAnyKey.SetActive(false);
            }else if(count==80){
                pressaAnyKey.SetActive(true);
                count = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.Z)){
            clouds.state = CloudSpawner.State.Wind;
            buttonPushed = true;
        }
    }
}
