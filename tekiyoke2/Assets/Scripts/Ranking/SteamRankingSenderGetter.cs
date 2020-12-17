using System;
using System.Collections.Generic;
using System.Linq;
using Ranking;
using ResultScene;
using Steamworks;
using UniRx;
using UnityEngine;

public class SteamRankingSenderGetter : MonoBehaviour, IRankingSenderGetter
{
    public void SendRanking(RankKind kind, float time, Action onSent)
    {
        throw new NotImplementedException();
    }

    public void GetRanking(RankKind kind, Action<RankData> onGot)
    {
        if(! SteamManager.Initialized) return;
        
        CallResult<LeaderboardFindResult_t>
        .Create()
        .Set
        (
            SteamUserStats.FindLeaderboard(kind.ToString()),
            (result, failure) => OnLeaderboardFound(result, failure, onGot, kind)
        );
    }

    void OnLeaderboardFound(LeaderboardFindResult_t result, bool failure, Action<RankData> onGot, RankKind kind)
    {
        if (failure)
        {
            print("リーダーボードの取得失敗");
            return;
        }
        if (result.m_bLeaderboardFound == 0)
        {
            print("そんなランキングはない");
            return;
        }
        
        DownloadScores(result, onGot, kind);
    }
    
    void DownloadScores(LeaderboardFindResult_t result, Action<RankData> onGot, RankKind kind)
    {
        Subject<RankDatum[]> top100Got = new Subject<RankDatum[]>();
        Subject<RankDatum[]> aroundYou100Got = new Subject<RankDatum[]>();

        var downloadTop100 = SteamUserStats.DownloadLeaderboardEntries
        (
            result.m_hSteamLeaderboard,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal,
            1,
            100
        );
        CallResult<LeaderboardScoresDownloaded_t>
        .Create()
        .Set
        (
            downloadTop100,
            (scores_result, scores_failure) => OnScoresDownloaded(scores_result, scores_failure, top100Got.OnNext)
        );
        
        
        var downloadAroundYou100 = SteamUserStats.DownloadLeaderboardEntries
        (
            result.m_hSteamLeaderboard,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser,
            -50,
            50
        );
        CallResult<LeaderboardScoresDownloaded_t>
        .Create()
        .Set
        (
            downloadAroundYou100,
            (scores_result, scores_failure) => OnScoresDownloaded(scores_result, scores_failure, aroundYou100Got.OnNext)
        );
        
        top100Got.CombineLatest
        (
            aroundYou100Got,
            (top100, aroundYou100) => new RankData(kind, top100, aroundYou100)
        )
        .Subscribe(onGot);
    }

    void OnScoresDownloaded(LeaderboardScoresDownloaded_t result, bool failure, Action<RankDatum[]> onGot)
    {
        if (failure)
        {
            print("スコアのダウンロード失敗");
            return;
        }
        
        onGot.Invoke(ToDatum_s(result));
    }

    RankDatum[] ToDatum_s(LeaderboardScoresDownloaded_t result)
    {
        var datum_s = new List<RankDatum>();

        foreach(int i in Enumerable.Range(0, result.m_cEntryCount))
        {
            if(SteamUserStats.GetDownloadedLeaderboardEntry
            (
                result.m_hSteamLeaderboardEntries,
                i,
                out LeaderboardEntry_t leaderboardEntry,
                new int[0],
                0
            ))
            {
                datum_s.Add(new RankDatum
                (
                    SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser),
                    leaderboardEntry.m_nGlobalRank,
                    leaderboardEntry.m_nScore
                ));
            }
        }

        return datum_s.ToArray();
    }
}