using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CursorMove : MonoBehaviour
{
    [SerializeField] float width = 10;
    [SerializeField] float duration = 0.5f;
    [SerializeField] Ease ease = Ease.InOutSine;
    [SerializeField] Image backLight;
    [SerializeField] float backLightDur = 0.2f;

    Sequence seq;

    void Start()
    {
        seq = DOTween.Sequence();
        seq.Append(
            GetComponent<RectTransform>()
            .DOLocalMoveX(width,duration)
            .SetRelative()
            .SetEase(ease)
        );
        seq.Play().SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPushed(){
        seq.Pause();

        Sequence blseq = DOTween.Sequence();
        blseq.Append(
            backLight.DOFade(1, backLightDur/2).SetEase(Ease.OutQuint)
        );
        blseq.Append(
            backLight.DOFade(0.7f, backLightDur/2)
        );
    }
}
