using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RankingCategoryTab : MonoBehaviour
{
    [SerializeField] FocusNode focusNode;
    [SerializeField] Image dpImage;
    [SerializeField] float rotateSpeed = 200;

    void Awake()
    {
        focusNode.OnFocused.Subscribe(_ => dpImage.DOFade(1, 0.2f).SetEase(Ease.Linear));
        focusNode.OnUnFocused.Subscribe(_ => dpImage.DOFade(0, 0.2f).SetEase(Ease.Linear));
    }

    void Update()
    {
        if(focusNode.Focused) dpImage.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
