using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ZatsuTest : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().DOFade(0.7f,1f);
        transform.DOLocalMoveX(0,1f).SetEase(Ease.OutQuint);
    }
}
