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

    [FormerlySerializedAs("wakuLight")] [SerializeField] WakuMover waku;
    
    [SerializeField] SoundGroup soundGroup;

    [SerializeField] StageSelectBGChanger bgChanger;

    [SerializeField] AfterEffects afterEffects;

    [SerializeField] Texture2D draft2Light;
    [SerializeField] Texture2D draft3Light;

    [SerializeField] Texture2D draft2Black;
    [SerializeField] Texture2D draft3Black;

    #endregion

    enum State{ Entering, Active, Selected }
    State state = State.Entering;

    [SerializeField] IInput input;

    
    Subject<EDraft> _StageSelected = new Subject<EDraft>();
    public IObservable<EDraft> StageSelected => _StageSelected;
    
    Subject<Unit> _OnGoToConfig = new Subject<Unit>();
    public IObservable<Unit> OnGoToConfig => _OnGoToConfig;
    
    Subject<Unit> _OnGoToRankings = new Subject<Unit>();
    public IObservable<Unit> OnGoToRankings => _OnGoToRankings;

    PostEffectWrapper vignette;

    void Start()
    {
        foreach (FocusNode node in AllNodes)
        {
            node.OnFocused
                .Subscribe(_ =>
                {
                    waku.ChangeFocus(node.GetComponent<RectTransform>());
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
                soundGroup.Play("Enter");
                ExitMain();
                DOVirtual.DelayedCall(0.3f, () => _OnGoToRankings.OnNext(Unit.Default));
            })
            .AddTo(this);

        // 設定
        Selected(goToConfig)
            .Subscribe(_ =>
            {
                soundGroup.Play("Enter");
                ExitMain();
                DOVirtual.DelayedCall(0.3f, () => _OnGoToConfig.OnNext(Unit.Default));
            })
            .AddTo(this);

        vignette = afterEffects.Find("VignetteInStageSelect");
        vignette.SetVolume(0);
    }

    IObservable<Unit> Selected(FocusNode node)
    {
        return node.OnSelected.Where(_ => state == State.Active);
    }
    
    void OnDetermine(int index)
    {
        state = State.Selected;
        waku.Stop();
        soundGroup.Play("Enter");
        AllStages[index].GetComponent<Image>().DOFade(1, 0.3f).SetEase(Ease.Linear);
        
        _StageSelected.OnNext(EDraftUtil.ToEDraft(index));
    }

    void ExitMain()
    {
        FadeOut();
        focusManager.OnExit();
    }

    public void Enter(IReadOnlyList<bool> draftsSelectable, bool unlocking)
    {
        gameObject.SetActive(true);
        focusManager.OnEnter();
        goToConfig.gameObject.SetActive(true);
        goToRankings.gameObject.SetActive(true);
        
        ApplySelectableDrafts(draftsSelectable);

        if (unlocking)
        {
            if (!draftsSelectable[2])
            {
                if (draftsSelectable[1])
                {
                    UnlockDraft(draft2.GetComponent<Image>(), draft2Light, draft2Black);
                }
                else
                {
                    print("実際には通らない");
                }
            }
            else
            {
                UnlockDraft(draft3.GetComponent<Image>(), draft3Light, draft3Black);
            }
        }
        
        FadeIn();
        focusManager.OnEnter();
    }

    void ApplySelectableDrafts(IReadOnlyList<bool> draftsSelectable)
    {
        if (!draftsSelectable[1])
        {
            print("本来こうはならないけど開発中はなる");
            
            draft2.gameObject.SetActive(false);
            draft3.gameObject.SetActive(false);

            draft1.Down     = goToRankings;
            goToRankings.Up = draft1;
            goToConfig.Up   = draft1;
        }
        else if (!draftsSelectable[2])
        {
            draft3.gameObject.SetActive(false);

            draft2.Down     = goToRankings;
            goToRankings.Up = draft2;
            goToConfig.Up   = draft2;
        }
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
            AllStages
                .Select(node => node.GetComponent<Image>())
                .Concat(new Image[]{chooseADraftImage})
                .ForEach(img =>
                {
                    img.transform.SetLocalX(100);
                    img.DOFade(0, 0);
                });
            goToRanksImage.transform.SetLocalX(-50);
            goToRanksImage.DOFade(0, 0);
            goToConfigImage.transform.SetLocalX(270);
            goToConfigImage.DOFade(0, 0);
            waku.FadeOut(0, Ease.Linear);
        }

        // ステージのフェードイン
        stageImages.ForEach((stageImage, i) =>
        {
            const float slideGap = 0.067f;
            DOTween.Sequence()
                .AppendInterval(i * slideGap)
                .Append(stageImage.DOFade(targetAlpha, fadeInDuration).SetEase(Ease.Linear))
                .Join(stageImage.transform.DOLocalMoveX(0, fadeInDuration).SetEase(Ease.OutCubic));
        });
        
        // ランキング、設定
        DOTween.Sequence()
            .AppendInterval(0.3f)
            .Append(goToConfigImage.DOFade(targetAlpha, fadeInDuration).SetEase(Ease.Linear))
            .Join(goToConfigImage.transform.DOLocalMoveX(-100, fadeInDuration).SetRelative().SetEase(Ease.OutCubic))
            .Join(goToRanksImage.DOFade(targetAlpha, fadeInDuration).SetEase(Ease.Linear))
            .Join(goToRanksImage.transform.DOLocalMoveX(-100, fadeInDuration).SetRelative().SetEase(Ease.OutCubic));
            
        // その他
        DOTween.Sequence()
            .AppendInterval(0.5f)
            .Append(chooseADraftImage.DOFade(1, fadeInDuration).SetEase(Ease.Linear))
            .Join(chooseADraftImage.transform.DOLocalMoveX(0, fadeInDuration).SetEase(Ease.OutCubic))
            .AppendCallback(() => waku.Transform.position = focusManager.Focused.transform.position)
            .Append(waku.WakuImage.DOFade(1, 0.2f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                state = State.Active;
                waku.Start_();
            });
    }

    void UnlockDraft(Image unlockedImage, Texture2D lightTex, Texture2D blackTex)
    {
        focusManager.SetAcceptsInput(false);
        waku.gameObject.SetActive(false);
        
        // なんかインスタンス化されないので複製する(todo: 破棄)
        Material mat = new Material(unlockedImage.material);
        unlockedImage.material = mat;
        mat.SetTexture("_LightTex", lightTex);
        mat.SetTexture("_BlackTex", blackTex);
        
        mat.SetFloat("_Contrast",           1);
        mat.SetFloat("_LightAreaThreshold", -0.1f);
        mat.SetFloat("_Light",              3);
        
        DOTween.Sequence()
            .AppendInterval(0.5f)
            .Append(vignette.To(5, 0.8f).SetEase(Ease.OutCubic))
            .AppendCallback(() => soundGroup.Play("Light"))
            .Append(mat.To("_LightAreaThreshold", 1.1f, 2f).SetEase(Ease.Linear))
            .Append(vignette.To(-2, 0.5f).SetEase(Ease.OutSine))
            .Join(mat.To("_Contrast", 0, 0.5f).SetEase(Ease.OutSine))
            .Append(vignette.To(0, 0.3f).SetEase(Ease.InOutSine))
            .Join(mat.To("_Light", 0, 0.3f).SetEase(Ease.InOutSine))
            .OnComplete(() =>
            {
                focusManager.SetAcceptsInput(true);
                waku.gameObject.SetActive(true);
                waku.SetInvisible();
                waku.FadeIn(0.3f, Ease.InOutSine);
            });
    }

    void FadeOut()
    {
        float dur = 0.4f;

        AllNodes.Select(node => node.GetComponent<Image>())
            .Concat(new Image[]{ waku.WakuImage, chooseADraftImage })
            .ForEach(img =>
            {
                img.DOFade(0, dur).SetEase(Ease.OutCubic);
                img.transform.DOLocalMoveX(-100, dur).SetRelative().SetEase(Ease.OutCubic);
            });
        waku.LightImage.DOFade(0, dur).SetEase(Ease.OutCubic);
        
        waku.Stop();
    }
}
