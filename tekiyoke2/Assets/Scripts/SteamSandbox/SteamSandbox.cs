using System;
using Ranking;
using Steamworks;
using UnityEngine;

public class SteamSandbox : MonoBehaviour
{
    void Start()
    {
        if(! SteamManager.Initialized) return;
        
        CallResult<LeaderboardFindResult_t>
            .Create()
            .Set(SteamUserStats.FindLeaderboard(RankKind.Draft1.ToString()), OnLeaderboardFound);
    }

    void OnLeaderboardFound(LeaderboardFindResult_t result, bool failure)
    {
        if (failure)
        {
            print("failure");
            return;
        }
        
        if (result.m_bLeaderboardFound == 0)
        {
            print("leaderboard not found");
            return;
        }
        
        print($"リーダーボードの名前 : {SteamUserStats.GetLeaderboardName(result.m_hSteamLeaderboard)}");
        print("download entries...");
        var downloadLeaderboardEntries = SteamUserStats.DownloadLeaderboardEntries
        (
            result.m_hSteamLeaderboard,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser,
            -50,
            50
        );
        
        CallResult<LeaderboardScoresDownloaded_t>
            .Create()
            .Set(downloadLeaderboardEntries, OnEntriesGot);
    }

    void OnEntriesGot(LeaderboardScoresDownloaded_t result, bool failure)
    {
        print("downloaded!");
        if (failure)
        {
            print("failure");
            return;
        }
        
        Debug.Log($"取得した順位の個数 : {result.m_cEntryCount}");

        for (int i = 0; i < result.m_cEntryCount; i++)
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
                this.PrintLines
                (
                    $"順位     : {leaderboardEntry.m_nGlobalRank}",
                    $"ID       : {leaderboardEntry.m_steamIDUser}",
                    $"スコア   : {leaderboardEntry.m_nScore}",
                    $"ユーザ名 : {SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser)}"
                );
            }
        }
    }
}
