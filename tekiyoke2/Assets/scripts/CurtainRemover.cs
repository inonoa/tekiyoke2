using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainRemover : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(40,0);
        if(gameObject.transform.position.x>2000){
            gameObject.SetActive(false);
        }
    }
}
