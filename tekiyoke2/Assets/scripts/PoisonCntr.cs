using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCntr : MonoBehaviour
{
    public HpCntr hpcntr;


    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="Player"){
            hpcntr.HP = hpcntr.HP - 1;
            Debug.Log(other);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
