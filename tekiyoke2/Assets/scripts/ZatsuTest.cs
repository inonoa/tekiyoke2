using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ZatsuTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().DOFade(0.7f,1f);
        transform.DOLocalMoveX(0,1f).SetEase(Ease.OutQuint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
