using System;
using System.Linq;
using DG.Tweening;
using Ranking;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
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
        [SerializeField] IInput               input;
        [SerializeField] SoundGroup           sounds;
        
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
                }
            );

            DOVirtual.DelayedCall(3f, () =>
            {
                this.UpdateAsObservable()
                    .Where(_ => input.GetButtonDown(ButtonCode.Ranking))
                    .Subscribe(_ =>
                    {
                        sounds.Play("Put");
                        gameObject.SetActive(false);
                        rankView.Show(rankKind);
                    })
                    .AddTo(this);
                rankView.OnExit.Subscribe(_ => gameObject.SetActive(true));
            });
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
