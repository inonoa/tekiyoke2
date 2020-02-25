using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class BlueSlideIn : MonoBehaviour
{
    [SerializeField] float maxAlpha = 0.3f;
    [SerializeField] float moveSec = 1f;

    void Start()
    {
        transform.DOLocalMoveX(0, moveSec).SetEase(Ease.OutQuint);
        GetComponent<Image>().DOFade(maxAlpha, moveSec);
    }
}
