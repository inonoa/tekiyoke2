using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using ResultScene;
using UnityEngine;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;

namespace Ranking
{
    public class PlayfabRankingSenderGetter : MonoBehaviour, IRankingSenderGetter
    {
        [SerializeField] PlayFabLoginManager loginManager;
        
        public void SendRanking(RankKind kind, float time, Action onSent)
        {
            if (!loginManager.IsLoggedIn())
            {
                loginManager.Login(() => SendRanking(kind, time, onSent), error => print(error.GenerateErrorReport()));
                return;
            }
            
            int score = TimeToNumberForPlayFab(time);
            List<StatisticUpdate> stats = new List<StatisticUpdate>();
            stats.Add(new StatisticUpdate {StatisticName = kind.ToString(), Value = score});
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = stats
            };
            PlayFabClientAPI.UpdatePlayerStatistics
            (
                request,
                result => onSent.Invoke(),
                error  => Debug.LogError(error.GenerateErrorReport())
            );
        }

        public void GetRanking(RankKind kind, Action<RankData> onGot)
        {
            if (!loginManager.IsLoggedIn())
            {
                loginManager.Login(() => GetRanking(kind, onGot), error => print(error.GenerateErrorReport()));
                return;
            }
            
            StartCoroutine(GetRankingCor(kind, onGot));
        }

        IEnumerator GetRankingCor(RankKind kind, Action<RankData> onGot)
        {
            List<PlayerLeaderboardEntry> top100 = null;
            var requestTop100 = new GetLeaderboardRequest
            {
                StatisticName = kind.ToString(),
                MaxResultsCount = 100
            };
            PlayFabClientAPI.GetLeaderboard
            (
                requestTop100,
                result => top100 = result.Leaderboard,
                error => Debug.Log(error.GenerateErrorReport())
            );

            List<PlayerLeaderboardEntry> aroundPlayer100 = null;
            var requestAroundPlayer = new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = kind.ToString(),
                MaxResultsCount = 100
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer
            (
                requestAroundPlayer,
                result => aroundPlayer100 = result.Leaderboard,
                error  => Debug.Log(error.GenerateErrorReport())
            );

            yield return new WaitUntil(() => top100 != null && aroundPlayer100 != null);

            onGot.Invoke(new RankData
            (
                kind,
                top100
                    .Select(entry => new RankDatum(entry.DisplayName, entry.Position + 1, NumberToTimeFloat(entry.StatValue)))
                    .ToArray(),
                aroundPlayer100
                    .Select(entry => new RankDatum(entry.DisplayName, entry.Position + 1, NumberToTimeFloat(entry.StatValue)))
                    .ToArray()
            ));
        }
        
        const int TimeMax = Int32.MaxValue;
        
        [Button]
        static int TimeToNumberForPlayFab(float time)
        {
            return TimeMax - (int) (time * 1000);
        }

        [Button]
        static float NumberToTimeFloat(int numFromPlayFab)
        {
            return (TimeMax - numFromPlayFab) / 1000f;
        }
    }
}
