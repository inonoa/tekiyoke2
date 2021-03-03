using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTileController : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag(Tags.Hero))
        {
            HeroDefiner.currentHero.Damage(HeroDefiner.currentHero.HPController.HP, DamageType.Drop);
            HeroDefiner.currentHero.velocity.Y = Mathf.Max(HeroDefiner.currentHero.velocity.Y, -20);
        }
    }
}
