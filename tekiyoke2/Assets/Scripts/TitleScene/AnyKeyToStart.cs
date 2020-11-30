using System;
using System.Collections;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class AnyKeyToStart : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float fadeInSeconds = 0.35f;
    [SerializeField] float blinkSeconds = 0.7f;
    [SerializeField] float fadeOutSeconds = 0.35f;
    [SerializeField] float fadeOutMoveDistance = 100;

    Coroutine blinkCoroutine;
    
    public void Appear()
    {
        image.DOFade(1, fadeInSeconds).SetEase(Ease.OutQuint)
            .onComplete += () =>
        {
            blinkCoroutine = StartCoroutine(Blink());
        };
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkSeconds);
            image.DOFade(0, 0);
            yield return new WaitForSeconds(blinkSeconds);
            image.DOFade(1, 0);
        }
    }

    public void Disappear()
    {
        StopCoroutine(blinkCoroutine);
        image.DOFade(0, fadeOutSeconds);
        image.transform.DOLocalMoveX(-fadeOutMoveDistance, fadeOutSeconds).SetRelative();
    }

    void Start()
    {
        image.DOFade(0, 0);
    }
}
