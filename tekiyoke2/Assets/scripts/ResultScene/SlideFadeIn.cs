using UnityEngine;
using DG.Tweening;

public class SlideFadeIn : MonoBehaviour
{
    [SerializeField] float secToIn = 1;
    [SerializeField] float secDelay = 0;
    [SerializeField] Vector3 distanceVec = new Vector3(1000,0,0);
    
    void Start()
    {
        DOVirtual.DelayedCall(secDelay, () => {
            transform.DOLocalMove(distanceVec, secToIn).SetEase(Ease.OutQuint).SetRelative();
            GetComponent<CanvasGroup>().DOFade(1, secToIn);
        });
    }
}
