using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameTimeCounter clock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Player"){
            HeroDefiner.currentHero.spriteRenderer.enabled = false;
            HeroDefiner.currentHero.CanMove = false;
            clock.DoesTick = false;
            Debug.Log("GOAL!!!!");
        }
    }
}
