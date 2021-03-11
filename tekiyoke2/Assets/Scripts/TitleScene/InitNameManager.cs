using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InitNameManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] Button enterButton;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] SoundGroup sounds;

    void Awake()
    {
        enterButton.OnClickAsObservable().Subscribe(_ =>
        {
            sounds.Play("Push");
            _NameEntered.OnNext(inputField.text);
        });
    }

    public void Enter()
    {
        gameObject.SetActive(true);
    }

    public IObservable<Unit> Exit()
    {
        var end = new Subject<Unit>();
        
        canvasGroup.DOFade(0, 0.3f);
        transform.DOLocalMoveX(-30, 0.3f).SetEase(Ease.OutCubic);
        DOVirtual.DelayedCall(1.8f, () =>
        {
            gameObject.SetActive(false);
            end.OnNext(Unit.Default);
        });

        return end;
    }

    Subject<string> _NameEntered = new Subject<string>();
    public IObservable<string> NameEntered => _NameEntered;
}
