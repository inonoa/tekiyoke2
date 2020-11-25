using System;
using System.Collections;
using Ranking;
using ResultScene;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RankingsSelectManager : SerializedMonoBehaviour
{
    [SerializeField] Button draft1Button;
    [SerializeField] Button draft2Button;
    [SerializeField] Button draft3Button;
    [SerializeField] Button allDraftsButton;
    [SerializeField] IRankView rankView;
    [SerializeField] IRankingSenderGetter rankingSenderGetter;
    [SerializeField] Button exitButton;

    public void Enter()
    {
        gameObject.SetActive(true);
    }

    public IObservable<Unit> OnExit => _OnExit;
    Subject<Unit> _OnExit = new Subject<Unit>();
    
    EnumArray<RankData, RankKind> rankDatas = new EnumArray<RankData, RankKind>();

    void Start()
    {
        draft1Button   .onClick.AddListener(() => rankView.Show(RankKind.Draft1));
        draft2Button   .onClick.AddListener(() => rankView.Show(RankKind.Draft2));
        draft3Button   .onClick.AddListener(() => rankView.Show(RankKind.Draft3));
        allDraftsButton.onClick.AddListener(() => rankView.Show(RankKind.AllDrafts));

        foreach (object kind_ in Enum.GetValues(typeof(RankKind)))
        {
            RankKind kind = (RankKind) kind_;
            rankingSenderGetter.GetRanking(kind, data => rankView.SetData(data));
        }
        
        exitButton.onClick.AddListener(Exit);
    }

    void Exit()
    {
        gameObject.SetActive(false);
        _OnExit.OnNext(Unit.Default);
    }
}