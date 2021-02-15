using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class WarpDoor : MonoBehaviour
{
    [SerializeField] bool right;

    Subject<WarpDoor> _HeroEnter = new Subject<WarpDoor>();
    public IObservable<WarpDoor> HeroEnter => _HeroEnter;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Tags.HeroCenter))
        {
            bool fromRight = HeroDefiner.currentHero.velocity.X < 0;
            if (fromRight == right)
            {
                _HeroEnter.OnNext(this);
            }
        }
    }
}
