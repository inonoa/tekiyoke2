using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Sirenix.OdinInspector;

public class Chishibuki : MonoBehaviour
{
    [SerializeField] int fadeinFrames = 10;
    [SerializeField] int chishibukiFrames = 30;
    [SerializeField] int fadeoutFrames = 30;
    [SerializeField] Image image;

    [SerializeField][ReadOnly] bool canChishibuki = true;

    public void StartChishibuki()
    {
        if(!canChishibuki) return;

        canChishibuki = false;
        image.gameObject.SetActive(true);
        image.color = new Color(1,1,1,0);

        DOTween.Sequence()
            .Append(image.DOFade(1, fadeinFrames  / 60f))
            .AppendInterval(chishibukiFrames /  60f)
            .Append(image.DOFade(0, fadeoutFrames / 60f))
            .OnComplete(() =>
            {
                image.gameObject.SetActive(false);
                canChishibuki = true;
            })
            .SetUpdate(true)
            .GetPausable().AddTo(this);
    }
}
