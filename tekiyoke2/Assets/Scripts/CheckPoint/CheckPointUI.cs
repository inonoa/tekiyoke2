using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class CheckPointUI : MonoBehaviour
{
    [SerializeField] CheckPointsManager checkPointsManager;
    [SerializeField] Text text;

    Tween currentTween;

    
    void Start()
    {
        checkPointsManager.PassedNewCheckPoint.Subscribe(checkPoint =>
        {
            Vector3 defaultPos = text.transform.localPosition;
            text.DOFade(0, 0);
            text.text = $"チェックポイント: {checkPoint.Name} を通過";

            currentTween = DOTween.Sequence()
                .Append
                (
                    text.transform
                        .DOLocalMoveX(-100, 1f)
                        .SetRelative()
                        .SetEase(Ease.OutQuint)
                )
                .Join
                (
                    text
                        .DOFade(1, 1f)
                )
                .AppendInterval(1f)
                .Append
                (
                    text.transform
                        .DOLocalMoveX(-100, 1f)
                        .SetRelative()
                        .SetEase(Ease.InOutSine)
                )
                .Join
                (
                    text
                        .DOFade(0, 1f)
                )
                .OnComplete(() =>
                {
                    text.transform.localPosition = defaultPos;
                });
        });

        Pauser.Instance.OnPause.Subscribe(_ =>
        {
            if(currentTween != null && currentTween.IsPlaying())
            {
                currentTween.Pause();
            }
        });
        Pauser.Instance.OnPauseEnd.Subscribe(_ =>
        {
            if(currentTween != null && currentTween.IsActive())
            {
                currentTween.TogglePause();
            }
        });
    }
}
