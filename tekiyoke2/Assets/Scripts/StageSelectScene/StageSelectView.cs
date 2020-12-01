using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageSelectView : SerializedMonoBehaviour, IStageSelectView
{   
    #region Objects
    [SerializeField] Image chooseADraftImage;
    [SerializeField] Image wakuImage;
    [SerializeField] WakuLightMover wakuLight;
    [SerializeField] Image[] stageImages;
    [SerializeField] SoundGroup soundGroup;

    [SerializeField] StageSelectBGChanger bgChanger;

    [SerializeField] Button goToRankingsButton;
    [SerializeField] Button goToConfigButton;

    #endregion

    #region States
    enum State{ Entering, Active, Selected }
    State state = State.Entering;
    EDraft selected = EDraft.Draft1;
    #endregion

    [SerializeField] IAskedInput input;

    
    Subject<EDraft> _StageSelected = new Subject<EDraft>();
    public IObservable<EDraft> StageSelected => _StageSelected;
    
    Subject<Unit> _OnGoToConfig = new Subject<Unit>();
    public IObservable<Unit> OnGoToConfig => _OnGoToConfig;
    
    Subject<Unit> _OnGoToRankings = new Subject<Unit>();
    public IObservable<Unit> OnGoToRankings => _OnGoToRankings;

    void Start()
    {
        goToRankingsButton.onClick.AddListener(() =>
        {
            ExitMain();
            _OnGoToRankings.OnNext(Unit.Default);
        });
        
        goToConfigButton.onClick.AddListener(() =>
        {
            ExitMain();
            _OnGoToConfig.OnNext(Unit.Default);
        });

        DOVirtual.DelayedCall(1f, () =>
        {
            goToRankingsButton.gameObject.SetActive(true);
            goToConfigButton.gameObject.SetActive(true);
        });
        
        FadeIn();
    }

    void ExitMain()
    {
        gameObject.SetActive(false);
        goToConfigButton.gameObject.SetActive(false);
        goToRankingsButton.gameObject.SetActive(false);
    }

    public void Enter()
    {
        gameObject.SetActive(true);
        goToConfigButton.gameObject.SetActive(true);
        goToRankingsButton.gameObject.SetActive(true);
    }

    void FadeIn()
    {
        const float targetAlpha = 0.8f;
        const float fadeInDuration = 0.4f;
        
        foreach (Image stageImage in stageImages)
        {
            Sequence fadeIn = DOTween.Sequence()
                //リセット
                .Append(stageImage.DOFade(0, 0))
                .Join(stageImage.transform.DOLocalMoveX(100, 0))
                .Join(wakuImage.DOFade(0, 0))
                .Join(wakuLight.GetComponent<Image>().DOFade(0, 0))
                //ステージ名部分
                .Append(stageImage.DOFade(targetAlpha, fadeInDuration).SetEase(Ease.Linear))
                .Join(stageImage.transform.DOLocalMoveX(0, fadeInDuration).SetEase(Ease.OutCubic))
                //枠
                .Append(wakuImage.DOFade(1, 0.2f).SetEase(Ease.Linear))
                .AppendCallback(() => state = State.Active);
        }
    }

    void MoveStage(EDraft dst)
    {
        selected = dst;
        float dstY = stageImages[dst.ToInt()].transform.position.y;
        wakuImage.transform.DOMoveY(dstY, 0.3f);
        bgChanger.OnChangeStage(selected.ToInt());
        soundGroup.Play("Move");
    }

    void OnDetermine()
    {
        state = State.Selected;
        wakuLight.Stop();
        soundGroup.Play("Enter");
        stageImages[selected.ToInt()].DOFade(1, 0.3f).SetEase(Ease.Linear);
        
        _StageSelected.OnNext(selected);
    }
    

    void Update()
    {
        if(state != State.Active) return;
        
        if(input.GetButtonDown(ButtonCode.Up) && selected != EDraft.Draft1)
        {
            MoveStage(selected.Minus1());
        }
        if(input.GetButtonDown(ButtonCode.Down) && selected != EDraft.Draft3)
        {
            MoveStage(selected.Plus1());
        }
        if(input.GetButtonDown(ButtonCode.Enter))
        {
            OnDetermine();
        }
    }
}
