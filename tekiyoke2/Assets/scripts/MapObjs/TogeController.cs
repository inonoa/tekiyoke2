using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogeController : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag=="Player") HeroDefiner.currentHero.Damage(1, DamageType.Normal);
    }
}
