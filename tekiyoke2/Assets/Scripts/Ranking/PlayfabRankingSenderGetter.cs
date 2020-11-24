using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using ResultScene;
using UnityEngine;
using PlayFab.ClientModels;

namespace Ranking
{
    public class PlayfabRankingSenderGetter : MonoBehaviour, IRankingSenderGetter
    {
        public void SendGetRanking(RankKind kind, float time, Action<RankData> onGot)
        {
            Login(_ => SendGetRankingBody());

            void SendGetRankingBody()
            {
                int milliseconds = (int) (time * 1000);
                List<StatisticUpdate> stats = new List<StatisticUpdate>();
                stats.Add(new StatisticUpdate {StatisticName = kind.ToString(), Value = milliseconds});
                var request = new UpdatePlayerStatisticsRequest
                {
                    Statistics = stats
                };
                PlayFabClientAPI.UpdatePlayerStatistics
                (
                    request,
                    result =>
                    {
                        print("sent!");
                        StartCoroutine(GetRanking(kind, onGot));
                    },
                    error  => Debug.LogError(error.GenerateErrorReport())
                );
            }
        }

        IEnumerator GetRanking(RankKind kind, Action<RankData> onGot)
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
                    .Select(entry => new RankDatum(entry.DisplayName, entry.Position, entry.StatValue / (float)1000))
                    .ToArray(),
                aroundPlayer100
                    .Select(entry => new RankDatum(entry.DisplayName, entry.Position, entry.StatValue / (float)1000))
                    .ToArray()
            ));
        }
        
        void Login(Action<LoginResult> resultCallback, Action<PlayFabError> errorCallback = null)
        {
            var request = new LoginWithCustomIDRequest()
            {
                CustomId = SystemInfo.deviceUniqueIdentifier
            };
            PlayFabClientAPI.LoginWithCustomID
            (
                request,
                resultCallback,
                errorCallback
            );
        }
    }
}