using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearToEnemyChecker : MonoBehaviour
{
    public HeroMover hero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Player"){
            Tokitome.SetTime(0.1f);
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag=="Player"){
            Tokitome.SetTime(1);
        }
    }
}
