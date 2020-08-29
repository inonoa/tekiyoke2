using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTileController : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            HeroDefiner.currentHero.Damage(HeroDefiner.currentHero.HpCntr.HP, DamageType.Drop);
            HeroDefiner.currentHero.velocity.Y = Mathf.Max(HeroDefiner.currentHero.velocity.Y, -20);
        }
    }
}
