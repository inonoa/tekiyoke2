using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeroAfterimage : MonoBehaviour, IReusable
{
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] float posZFromHero;

    public bool InUse{ get; private set; } = false;
    public void Activate(string _)
    {
        transform.position = new Vector3
        (
            HeroDefiner.CurrentPos.x,
            HeroDefiner.CurrentPos.y,
            HeroDefiner.CurrentPos.z + posZFromHero
        );

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(
            renderer.color.r,
            renderer.color.g,
            renderer.color.b,
            0.7f
        );

        renderer
            .DOFade(0, fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => 
            {
                InUse = false;
                gameObject.SetActive(false);
            })
            .AsHeros();
    }
}
