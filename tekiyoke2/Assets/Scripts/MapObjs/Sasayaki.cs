using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sasayaki : MonoBehaviour
{
    [SerializeField] float slideDistance = 30;
    [SerializeField] float duration = 0.5f;
    [SerializeField] SpriteRenderer bgSpr;
    [SerializeField] SpriteRenderer[] spritesExceptBG;
    readonly List<Tween> tweens = new List<Tween>();
    Vector3 defaultPos;
    float defaultAlpha;

    void Start()
    {
        defaultPos = transform.position;
        defaultAlpha = GetComponent<SpriteRenderer>().color.a;

        bgSpr.SetAlpha(0);
        spritesExceptBG.ForEach(spr => spr.SetAlpha(0));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Tags.Hero))
        {
            ClearTweens();

            float actualDist = (HeroDefiner.currentHero.velocity.X > 0) ? slideDistance : - slideDistance;
            transform.position = defaultPos + new Vector3(actualDist, 0, 0);
            tweens.Add(transform.DOMoveX(defaultPos.x, duration).SetEase(Ease.OutQuint));

            tweens.Add(bgSpr.DOFade(defaultAlpha, duration).SetEase(Ease.OutQuint));
            spritesExceptBG.ForEach(
                spr => tweens.Add(spr.DOFade(1, duration))
            );
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag(Tags.Hero))
        {
            ClearTweens();

            float actualDist = (HeroDefiner.currentHero.velocity.X > 0) ? slideDistance : - slideDistance;
            tweens.Add(transform.DOMoveX(defaultPos.x - actualDist, duration).SetEase(Ease.OutQuint));

            tweens.Add(bgSpr.DOFade(0, duration).SetEase(Ease.OutQuint));
            spritesExceptBG.ForEach(
                spr => tweens.Add(spr.DOFade(0, duration))
            );
        }
    }

    void ClearTweens()
    {
        tweens.ForEach(tw => tw.Kill());
        tweens.Clear();
    }
}
