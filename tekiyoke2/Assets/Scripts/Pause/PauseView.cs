using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Config;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UniRx.Triggers;

public class PauseView : MonoBehaviour
{
    [SerializeField] UIFocusManager focusManager;
    [SerializeField] FocusNode resume;
    [SerializeField] FocusNode goToConfig;
    [SerializeField] FocusNode quit;
    
    [SerializeField] Image draftName;
    [SerializeField] Image pausePause;
    [SerializeField] Image options;
    [SerializeField] Image mark;
    [SerializeField] float markRotateSpeed = 150;

    [SerializeField] ConfigManager config;

    [SerializeField, ReadOnly, FoldoutGroup("DefaultPositions")] float draftNameX;
    [SerializeField, ReadOnly, FoldoutGroup("DefaultPositions")] float pausePauseX;
    [SerializeField, ReadOnly, FoldoutGroup("DefaultPositions")] float optionsX;
    [SerializeField, ReadOnly, FoldoutGroup("DefaultPositions")] float markX;
    [Button, FoldoutGroup("DefaultPositions")]
    void ApplyDefaultPositions()
    {
        draftNameX  = draftName.transform.localPosition.x;
        pausePauseX = pausePause.transform.localPosition.x;
        optionsX    = options.transform.localPosition.x;
        markX       = mark.transform.localPosition.x;
    }
    
    RectTransform markRTF;
    SoundGroup soundGroup;
    
    Subject<Unit> _OnPauseEnd = new Subject<Unit>();
    public IObservable<Unit> OnPauseEnd => _OnPauseEnd;
    
    IInput input;

    public void Init(IInput input)
    {
        this.input = input;
    }

    void Start()
    {
        markRTF = mark.GetComponent<RectTransform>();
        soundGroup = GetComponent<SoundGroup>();

        focusManager.OnNodeFocused.Subscribe(node =>
        {
            var cursorPos = node.GetComponent<CursorPositionHolder>();
            mark.transform.position = cursorPos.CursorPosTransform.position;
            soundGroup.Play("Move");
        });
        this.UpdateAsObservable()
            .Subscribe(_ => markRTF.Rotate(0, 0, markRotateSpeed * Time.deltaTime));

        resume.OnSelected.Subscribe(_ =>
        {
            soundGroup.Play("Enter");
            _OnPauseEnd.OnNext(Unit.Default);
        });
        goToConfig.OnSelected.Subscribe(_ =>
        {
            soundGroup.Play("Enter");
            Hide();
            config.Enter();
        });
        quit.OnSelected.Subscribe(_ =>
        {
            soundGroup.Play("Enter");
            SceneTransition.Start2ChangeScene("StageChoiceScene", SceneTransition.TransitionType.Default);
        });

        config.OnExit.Subscribe(_ => Enter());
    }

    public void Enter()
    {
        const float dur = 0.4f;
        const float delay = 0.1f;
        const float dist = 100;
        
        new[] {(draftName, draftNameX), (options, optionsX), (pausePause, pausePauseX), (mark, markX)}
        .ForEach(img_defX =>
        {
            (Image img, float defaultX) = img_defX;
            
            img.transform.SetLocalX(defaultX + (img == pausePause ? dist : -dist));
            img.SetAlpha(0);

            DOVirtual.DelayedCall(delay, () =>
            {
                img
                    .DOFade(1, dur)
                    .SetEase(Ease.OutCubic);
                img.transform
                    .DOLocalMoveX(defaultX, dur)
                    .SetEase(Ease.OutCubic);
            });
            DOVirtual.DelayedCall(delay + dur, () => focusManager.OnEnter());
        });
    }

    void Hide()
    {
        focusManager.OnExit();
        
        const float dur = 0.4f;
        const float dist = 100;
        
        new[] {(draftName, draftNameX), (options, optionsX), (pausePause, pausePauseX), (mark, markX)}
        .ForEach(img_defX =>
        {
            (Image img, float defaultX) = img_defX;
            
            img
                .DOFade(0, dur)
                .SetEase(Ease.OutCubic);
            img.transform
                .DOLocalMoveX(defaultX + ((img == pausePause) ? dist : -dist), dur)
                .SetEase(Ease.OutCubic);
        });
    }
}
