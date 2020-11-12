using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class KieruYuka : MonoBehaviour
{
    [SerializeField] float touchToVanishSecs  = 0.7f;
    [SerializeField] float vanishToAppearSecs = 1.7f;

    [Space(10)]
    [SerializeField] Sprite kieruSprite;
    [SerializeField] Sprite kieteruSprite;
    [SerializeField] Sprite kienaiSprite;

    SpriteRenderer spriteRenderer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Player")
        {
            ReadyToVanish();
        }
    }

    void ReadyToVanish()
    {
        DOVirtual.DelayedCall(0.1f, () =>
        {
            spriteRenderer.sprite = kieruSprite;
        });
        DOVirtual.DelayedCall(touchToVanishSecs - 0.25f, () =>
        {
            spriteRenderer.sprite = kieteruSprite;
        });
        DOVirtual.DelayedCall(touchToVanishSecs, () =>
        {
            gameObject.SetActive(false);
            ReadyToAppear();
        });
    }

    void ReadyToAppear()
    {
        DOVirtual.DelayedCall(vanishToAppearSecs, () =>
        {
            gameObject.SetActive(true);
            spriteRenderer.sprite = kienaiSprite;
        });
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
