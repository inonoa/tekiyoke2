using System;
using Ranking;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ResultScene
{
    public class ResultSceneManager : SerializedMonoBehaviour
    {
        [SerializeField] ScoreHolder          scoreHolder;
        [SerializeField] SaveDataManager      saveDataManager;
        [SerializeField] ScoresToText         scoreToText;
        [SerializeField] IRankingSenderGetter rankingSenderGetter;
        [SerializeField] IRankView            rankView;
        
        void Start()
        {
            StagePlayData playData = scoreHolder.Get();
            (bool isFirstPlay, float lastBestTime) = saveDataManager.SetStageData(playData);
            scoreToText.Init(playData.Stage, playData.Time, isFirstPlay, lastBestTime);
            rankingSenderGetter.SendGetRanking
            (
                RankKindUtil.ToKind(playData.Stage),
                playData.Time,
                data => rankView.Show(data)
            );
        }
    }

    public interface IRankingSenderGetter
    {
        void SendGetRanking(RankKind kind, float time, Action<RankData> onGot);
    }

    public interface IRankView
    {
        void Show(RankData rankData);
    }
}