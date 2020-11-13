using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaneController : MonoBehaviour
{
    [SerializeField] float fromTriggerToFly = 1f;
    float seconds2Fly = 0;
    [SerializeField] float jumpForce = 60;

    bool touchedLast = false;

    [Space(10)]
    [SerializeField] ContactFilter2D filter = new ContactFilter2D();
    Collider2D col;

    [SerializeField] SpriteRenderer shaderSpriteRenderer;
    Material mat;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        mat = shaderSpriteRenderer.material;
    }

    void Update()
    {
        if(col.IsTouching(filter))
        {
            if(!touchedLast)
            {
                touchedLast = true;
                mat.SetInt("_HeroOn", 1);
                seconds2Fly = fromTriggerToFly;
            }

            if(seconds2Fly <= 0) return;
            seconds2Fly -= TimeManager.Current.DeltaTimeExceptHero;
            if(seconds2Fly <= 0)
            {
                HeroDefiner.currentHero.ForceJump(jumpForce);
                mat.SetInt("_Flash", 1);
                DOVirtual.DelayedCall(0.2f, () => mat.SetInt("_Flash", 0));
            }
        }
        else
        {
            if(touchedLast)
            {
                touchedLast = false;
                mat.SetInt("_HeroOn", 0);
            }
        }
    }
}
