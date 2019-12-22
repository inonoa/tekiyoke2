using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NaitrumCollider : MonoBehaviour
{
    public event EventHandler turn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Terrain"){
            turn?.Invoke(this,EventArgs.Empty);
        }
    }
}
