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
        GetComponent<Image>().DOFade(1,1f);
        transform.DOLocalMoveX(400,1f).SetEase(Ease.OutQuint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
