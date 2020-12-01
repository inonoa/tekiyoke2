using System;
using System.Collections;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class AnyKeyToStart : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float fadeInDelay = 2.5f;
    [SerializeField] float fadeInSeconds = 0.35f;
    [SerializeField] float blinkSeconds = 0.7f;
    [SerializeField] float fadeOutSeconds = 0.35f;
    [SerializeField] float fadeOutMoveDistance = 100;

    Coroutine blinkCoroutine;
    
    Subject<Unit> _FadeInCompleted = new Subject<Unit>();
    public IObservable<Unit> FadeInCompleted => _FadeInCompleted;
    
    void Appear()
    {
        image.DOFade(1, fadeInSeconds).SetEase(Ease.Linear)
            .onComplete += () =>
        {
            blinkCoroutine = StartCoroutine(Blink());
            _FadeInCompleted.OnNext(Unit.Default);
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
        DOVirtual.DelayedCall(fadeInDelay, Appear);
    }
}
