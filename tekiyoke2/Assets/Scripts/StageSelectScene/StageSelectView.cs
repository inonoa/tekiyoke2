using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageSelectView : SerializedMonoBehaviour, IStageSelectView
{   
    #region Objects
    
    [SerializeField] Image chooseADraftImage;
    
    [SerializeField] UIFocusManager focusManager;
    [SerializeField] FocusNode draft1;
    [SerializeField] FocusNode draft2;
    [SerializeField] FocusNode draft3;
    [SerializeField] FocusNode goToRankings;
    [SerializeField] FocusNode goToConfig;
    IReadOnlyList<FocusNode> AllNodes => new[] {draft1, draft2, draft3, goToConfig, goToRankings};

    IReadOnlyList<FocusNode> AllStages => new[] {draft1, draft2, draft3};

    [SerializeField] Image wakuImage;
    [SerializeField] WakuLightMover wakuLight;
    
    [SerializeField] SoundGroup soundGroup;

    [SerializeField] StageSelectBGChanger bgChanger;

    #endregion

    enum State{ Entering, Active, Selected }
    State state = State.Entering;

    [SerializeField] IAskedInput input;

    
    Subject<EDraft> _StageSelected = new Subject<EDraft>();
    public IObservable<EDraft> StageSelected => _StageSelected;
    
    Subject<Unit> _OnGoToConfig = new Subject<Unit>();
    public IObservable<Unit> OnGoToConfig => _OnGoToConfig;
    
    Subject<Unit> _OnGoToRankings = new Subject<Unit>();
    public IObservable<Unit> OnGoToRankings => _OnGoToRankings;

    void Start()
    {
        foreach (FocusNode node in AllNodes)
        {
            node.OnFocused
                .Skip(node == draft1 ? 1 : 0)
                .Subscribe(_ =>
                {
                    float dur = 0.3f;
                    RectTransform rect = node.GetComponent<RectTransform>();
                    wakuImage.rectTransform.DOSizeDelta(rect.sizeDelta, dur).SetEase(Ease.OutQuint);
                    wakuImage.rectTransform.DOMove(rect.position, dur).SetEase(Ease.OutQuint);
                    wakuLight.ChangeFocus(node.GetComponent<RectTransform>(), dur, Ease.OutQuint);
                    
                    soundGroup.Play("Move");
                })
                .AddTo(this);
        }

        draft1.OnFocused.Skip(1).Subscribe(_ => bgChanger.OnChangeStage(0));
        draft2.OnFocused.Subscribe(_ => bgChanger.OnChangeStage(1));
        draft3.OnFocused.Subscribe(_ => bgChanger.OnChangeStage(2));
        
        // ステージ選択
        AllStages.ForEach((stage, i) =>
        {
            Selected(stage)
                .Subscribe(_ => OnDetermine(i))
                .AddTo(this);
        });

        // ランキング
        Selected(goToRankings)
            .Subscribe(_ =>
            {
                goToRankings.GetComponent<Button>().onClick.Invoke();
                ExitMain();
                _OnGoToRankings.OnNext(Unit.Default);
            })
            .AddTo(this);

        // 設定
        Selected(goToConfig)
            .Subscribe(_ =>
            {
                goToConfig.GetComponent<Button>().onClick.Invoke();
                ExitMain();
                _OnGoToConfig.OnNext(Unit.Default);
            })
            .AddTo(this);
    }

    IObservable<Unit> Selected(FocusNode node)
    {
        return node.UpdateAsObservable()
            .Where(_ => node.Focused)
            .Where(_ => input.GetButtonDown(ButtonCode.Enter))
            .Where(_ => state == State.Active);
    }
    
    void OnDetermine(int index)
    {
        state = State.Selected;
        wakuLight.Stop();
        soundGroup.Play("Enter");
        AllStages[index].GetComponent<Image>().DOFade(1, 0.3f).SetEase(Ease.Linear);
        
        _StageSelected.OnNext(EDraftUtil.ToEDraft(index));
    }

    void ExitMain()
    {
        gameObject.SetActive(false);
        focusManager.OnExit();
        goToConfig.gameObject.SetActive(false);
        goToRankings.gameObject.SetActive(false);
    }

    public void Enter()
    {
        gameObject.SetActive(true);
        focusManager.OnEnter();
        goToConfig.gameObject.SetActive(true);
        goToRankings.gameObject.SetActive(true);
        
        FadeIn();
        focusManager.OnEnter();
    }

    void FadeIn()
    {
        const float targetAlpha = 0.8f;
        const float fadeInDuration = 0.3f;

        IEnumerable<Image> stageImages = AllStages.Select(stage => stage.GetComponent<Image>());
        Image goToRanksImage = goToRankings.GetComponent<Image>();
        Image goToConfigImage = goToConfig.GetComponent<Image>();
            
        // 初期位置に
        {
            AllNodes
                .Select(node => node.GetComponent<Image>())
                .Concat(new Image[]{chooseADraftImage})
                .ForEach(img =>
                {
                    img.transform.DOLocalMoveX(100, 0).SetRelative();
                    img.DOFade(0, 0);
                });
            wakuImage.DOFade(0, 0);
            wakuLight.GetComponent<Image>().DOFade(0, 0);
        }

        // ステージのフェードイン
        stageImages.ForEach((stageImage, i) =>
        {
            const float slideGap = 0.1f;
            DOTween.Sequence()
                .AppendInterval(i * slideGap)
                .Append(stageImage.DOFade(targetAlpha, fadeInDuration).SetEase(Ease.Linear))
                .Join(stageImage.transform.DOLocalMoveX(0, fadeInDuration).SetEase(Ease.OutCubic));
        });
        
        // ランキング、設定
        DOTween.Sequence()
            .AppendInterval(0.45f)
            .Append(goToConfigImage.DOFade(targetAlpha, fadeInDuration).SetEase(Ease.Linear))
            .Join(goToConfigImage.transform.DOLocalMoveX(-100, fadeInDuration).SetRelative().SetEase(Ease.OutCubic))
            .Join(goToRanksImage.DOFade(targetAlpha, fadeInDuration).SetEase(Ease.Linear))
            .Join(goToRanksImage.transform.DOLocalMoveX(-100, fadeInDuration).SetRelative().SetEase(Ease.OutCubic));
            
        // その他
        DOTween.Sequence()
            .AppendInterval(0.8f)
            .Append(chooseADraftImage.DOFade(1, fadeInDuration).SetEase(Ease.Linear))
            .Join(chooseADraftImage.transform.DOLocalMoveX(0, fadeInDuration).SetEase(Ease.OutCubic))
            .Append(wakuImage.DOFade(1, 0.2f).SetEase(Ease.Linear))
            .AppendCallback(() => state = State.Active);
    }
}
