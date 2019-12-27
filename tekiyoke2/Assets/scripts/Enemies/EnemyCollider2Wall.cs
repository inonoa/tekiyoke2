using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyCollider2Wall : MonoBehaviour
{
    public event EventHandler touched2Wall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        Debug.Log(other);
        if(other.gameObject.tag=="Terrain"){
            touched2Wall?.Invoke(this,EventArgs.Empty);
        }
    }
}
