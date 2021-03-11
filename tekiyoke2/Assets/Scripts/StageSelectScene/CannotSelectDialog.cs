using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CannotSelectDialog : MonoBehaviour
{
    [SerializeField] Image image;

    void Awake()
    {
        image.SetAlpha(0);

        const float fadeInDur = 0.3f;
        image.DOFade(0.8f, fadeInDur).SetEase(Ease.Linear);
        transform.DOLocalMoveY(-50, fadeInDur).From().SetEase(Ease.OutCubic);

        const float stayDur = 0.9f;
        DOVirtual.DelayedCall(fadeInDur + stayDur, () =>
        {
            const float fadeOutDur = 0.5f;
            image.DOFade(0, fadeOutDur).SetEase(Ease.Linear);
            transform.DOLocalMoveY(30, fadeOutDur).SetEase(Ease.InOutSine)
                .OnComplete(() => Destroy(gameObject));
        });
    }
}
