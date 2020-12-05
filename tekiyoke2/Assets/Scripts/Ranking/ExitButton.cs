using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image dpImage;
    [SerializeField] FocusNode focusNode;
    [SerializeField] float rotateSpeed = 200;
    
    Subject<Unit> _Pushed = new Subject<Unit>();
    public IObservable<Unit> Pushed => _Pushed;
    
    void Awake()
    {
        focusNode.OnFocused.Subscribe(_ =>
        {
            dpImage.DOFade(1, 0.2f).SetEase(Ease.Linear);
        });
        focusNode.OnUnFocused.Subscribe(_ =>
        {
            dpImage.DOFade(0, 0.2f).SetEase(Ease.Linear);
        });
        button.OnClickAsObservable().Subscribe(_ => _Pushed.OnNext(Unit.Default));
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        //
    }

    void Update()
    {
        if(!focusNode.Focused) return;

        dpImage.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _Pushed.OnNext(Unit.Default);
        }
    }
}
