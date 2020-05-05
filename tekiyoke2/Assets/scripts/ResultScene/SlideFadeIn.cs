using UnityEngine;
using DG.Tweening;

public class SlideFadeIn : MonoBehaviour
{
    [SerializeField] float secToIn = 1;
    [SerializeField] float secDelay = 0;
    [SerializeField] Vector3 distanceVec = new Vector3(100,0,0);
    [SerializeField] bool playsSE;
    
    void Start()
    {
        transform.position -= distanceVec;
        CanvasGroup canvasGrp = GetComponent<CanvasGroup>();
        canvasGrp.alpha = 0;
        DOVirtual.DelayedCall(secDelay, () => {
            transform.DOLocalMove(distanceVec, secToIn).SetEase(Ease.OutQuint).SetRelative();
            GetComponent<CanvasGroup>().DOFade(1, secToIn);
            if(playsSE) GetComponent<SoundGroup>().Play("In");
        });
    }
}
