using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyCollider : MonoBehaviour
{
    public HeroMover hero;
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
        if(other.gameObject.tag=="Player"){
            Tokitome.SetTime(1);
            hero.Damage(1);
        }
        if(other.gameObject.tag=="Terrain"){
            turn?.Invoke(this,EventArgs.Empty);
        }
    }
}
