using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Ranking;
using ResultScene;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RankingsSelectManager : SerializedMonoBehaviour
{
    [SerializeField] UIFocusManager focusManager;
    [SerializeField] WakuMover waku;
    [SerializeField] FocusNode draft1;
    [SerializeField] FocusNode draft2;
    [SerializeField] FocusNode draft3;
    [SerializeField] FocusNode allDrafts;
    [SerializeField] FocusNode exit;
    [SerializeField] SoundGroup sounds;
    [SerializeField] IRankView rankView;
    [SerializeField] IRankingSenderGetter rankingSenderGetter;

    public void Enter(IReadOnlyList<bool> draftsSelectable)
    {
        gameObject.SetActive(true);
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        transform.SetLocalX(100);
        waku.FadeOut(0, Ease.Linear);
        
        ApplySelectableDrafts(draftsSelectable);

        float dur = 0.4f;
        canvasGroup.DOFade(1, dur).SetEase(Ease.OutCubic);
        transform.DOLocalMoveX(0, dur).SetEase(Ease.OutCubic);
        DOVirtual.DelayedCall(dur, () =>
        {
            focusManager.OnEnter();
            waku.Start_();
            waku.FadeIn(0.4f, Ease.Linear);
        });
    }

    void ApplySelectableDrafts(IReadOnlyList<bool> draftsSelectable)
    {
        if (!draftsSelectable[1])
        {
            draft2.gameObject.SetActive(false);
            draft3.gameObject.SetActive(false);
            allDrafts.gameObject.SetActive(false);

            draft1.Down = exit;
            draft1.Left = exit;
            exit.Up     = draft1;
            exit.Right  = draft1;
        }
        else if (!draftsSelectable[2])
        {
            draft3.gameObject.SetActive(false);
            allDrafts.gameObject.SetActive(false);

            draft2.Down = exit;
            draft2.Left = exit;
            exit.Up     = draft2;
            exit.Right  = draft2;
        }
    }

    public IObservable<Unit> OnExit => _OnExit;
    Subject<Unit> _OnExit = new Subject<Unit>();
    
    EnumArray<RankData, RankKind> rankDatas = new EnumArray<RankData, RankKind>();

    void Start()
    {
        foreach (object kind_ in Enum.GetValues(typeof(RankKind)))
        {
            RankKind kind = (RankKind) kind_;
            rankingSenderGetter.GetRanking(kind, data => rankView.SetData(data));
        }

        (FocusNode node, RankKind kind)[] n_ks = new[]
        {
            (draft1, RankKind.Draft1),
            (draft2, RankKind.Draft2),
            (draft3, RankKind.Draft3),
            (allDrafts, RankKind.AllDrafts)
        };
        n_ks.ForEach(node_kind =>
        {
            node_kind.node.OnSelected.Subscribe(_ =>
            {
                rankView.Show(node_kind.kind);
                focusManager.OnExit();
                waku.Stop();
                sounds.Play("Enter");
            });
        });

        rankView.OnExit.Subscribe(_ =>
        {
            focusManager.OnEnter();
            waku.Start_();
        });
        
        exit.OnSelected.Subscribe(_ => Exit());

        new[] {draft1, draft2, draft3, allDrafts, exit}
        .ForEach(node =>
        {
            node.OnFocused.Subscribe(_ => OnFocused(node));
        });

        void OnFocused(FocusNode node)
        {
            waku.ChangeFocus(node.GetComponent<RectTransform>());
            sounds.Play("Move");
        }
    }

    void Exit()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        
        canvasGroup.DOFade(0, 0.4f).SetEase(Ease.OutCubic);
        transform.DOLocalMoveX(-100, 0.4f).SetEase(Ease.OutCubic);
        
        focusManager.OnExit();
        waku.Stop();
        sounds.Play("Enter");
        
        DOVirtual.DelayedCall(0.4f, () => gameObject.SetActive(false));
        DOVirtual.DelayedCall(0.2f, () => _OnExit.OnNext(Unit.Default));
    }
}