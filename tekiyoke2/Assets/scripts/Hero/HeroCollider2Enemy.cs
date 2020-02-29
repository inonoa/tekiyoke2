using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCollider2Enemy : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Enemy"){
            Tokitome.SetTime(1);
            HeroDefiner.currentHero.Damage(1);
        }
    }
}
