using System;
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

    void Start()
    {
        draft1Button   .onClick.AddListener(() => GoToRanking(RankKind.Draft1));
        draft2Button   .onClick.AddListener(() => GoToRanking(RankKind.Draft2));
        draft3Button   .onClick.AddListener(() => GoToRanking(RankKind.Draft3));
        allDraftsButton.onClick.AddListener(() => GoToRanking(RankKind.AllDrafts));
        
        exitButton.onClick.AddListener(Exit);
    }

    void GoToRanking(RankKind kind)
    {
        rankView.Show();
        rankingSenderGetter.GetRanking(kind, data => rankView.SetData(data));
    }

    void Exit()
    {
        gameObject.SetActive(false);
        _OnExit.OnNext(Unit.Default);
    }
}