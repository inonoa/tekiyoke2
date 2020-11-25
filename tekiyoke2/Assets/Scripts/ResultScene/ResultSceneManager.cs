using System;
using System.Linq;
using Ranking;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ResultScene
{
    public class ResultSceneManager : SerializedMonoBehaviour
    {
        [SerializeField] ScoreHolder          scoreHolder;
        [SerializeField] SaveDataManager      saveDataManager;
        [SerializeField] ScoresToText         scoreToText;
        [SerializeField] IRankingSenderGetter rankingSenderGetter;
        [SerializeField] IRankView            rankView;
        [SerializeField] Button               goToRankButton;
        
        void Start()
        {
            StagePlayData playData = scoreHolder.Get();
            (bool isFirstPlay, float lastBestTime) = saveDataManager.SetStageData(playData);
            scoreToText.Init(playData.Stage, playData.Time, isFirstPlay, lastBestTime);
            RankKind rankKind = RankKindUtil.ToKind(playData.Stage);

            if (saveDataManager.StageCleared.All(cleared => cleared))
            {
                rankingSenderGetter.SendRanking
                (
                    RankKind.AllDrafts,
                    saveDataManager.BestTimes.Sum(),
                    () => { }
                );
            }
            rankingSenderGetter.SendRanking
            (
                rankKind,
                playData.Time,
                () =>
                {
                    rankingSenderGetter.GetRanking
                    (
                        rankKind,
                        data => rankView.SetData(data)
                    );
                });
            goToRankButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                rankView.Show(rankKind);
            });
            rankView.OnExit.Subscribe(_ => gameObject.SetActive(true));
        }
    }

    public interface IRankingSenderGetter
    {
        void SendRanking(RankKind kind, float time, Action onSent);
        void GetRanking(RankKind kind, Action<RankData> onGot);
    }

    public interface IRankView
    {
        void SetData(RankData data);
        void Show(RankKind kind);
        IObservable<Unit> OnExit { get; }
    }
}
