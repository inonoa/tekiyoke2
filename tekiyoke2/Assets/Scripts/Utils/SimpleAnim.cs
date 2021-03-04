using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;
using UniRx;

public class SimpleAnim : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float changeSec = 0.1f;
    [SerializeField] bool loop;

    IEnumerator currentAnim;

    [Button]
    public void Play(Action onComplete = null)
    {
        if(currentAnim != null) StopCoroutine(currentAnim);
        
        var (subscription, enumerator) = this.StartPausableCoroutine(Anim(onComplete));
        subscription.AddTo(this);
        currentAnim = enumerator;
    }

    IEnumerator Anim(Action onComplete = null)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        
        while (true)
        {
            foreach (Sprite sprite in sprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(changeSec);
            }

            if (!loop)
            {
                onComplete?.Invoke();
                yield break;
            }
        }
    }
}
