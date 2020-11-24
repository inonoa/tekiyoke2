using System;
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
            rankingSenderGetter.SendGetRanking
            (
                RankKindUtil.ToKind(playData.Stage),
                playData.Time,
                data => rankView.SetData(data)
            );
            goToRankButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                rankView.Show();
            });
            rankView.OnExit.Subscribe(_ => gameObject.SetActive(true));
        }
    }

    public interface IRankingSenderGetter
    {
        void SendGetRanking(RankKind kind, float time, Action<RankData> onGot);
    }

    public interface IRankView
    {
        void SetData(RankData data);
        void Show();
        IObservable<Unit> OnExit { get; }
    }
}
