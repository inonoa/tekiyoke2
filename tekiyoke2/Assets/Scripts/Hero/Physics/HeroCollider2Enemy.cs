using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCollider2Enemy : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            HeroDefiner.currentHero.Damage(1, DamageType.Normal);
        }
    }
}
