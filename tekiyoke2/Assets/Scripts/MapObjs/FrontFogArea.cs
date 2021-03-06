using System;
using UnityEngine;

public class FrontFogArea : MonoBehaviour
{
    [SerializeField] FrontFogMover fog;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.HeroCenter))
        {
            var hero = other.GetComponentInParent<HeroMover>();
            if(hero is null) return;
            
            float heroX = hero.transform.position.x;
            bool heroIsLeft = heroX < this.transform.position.x;
            fog.FadeIn(heroIsLeft ? LR.L : LR.R);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.HeroCenter))
        {
            var hero = other.GetComponentInParent<HeroMover>();
            if(hero is null) return;
            
            float heroX = hero.transform.position.x;
            bool heroIsLeft = heroX < this.transform.position.x;
            fog.FadeOut(heroIsLeft ? LR.R : LR.L);
        }
    }
}