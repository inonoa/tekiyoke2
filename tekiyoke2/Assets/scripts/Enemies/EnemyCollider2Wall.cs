using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyCollider2Wall : MonoBehaviour
{
    public event EventHandler touched2Wall;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Terrain"){
            touched2Wall?.Invoke(this,EventArgs.Empty);
        }
    }
}
