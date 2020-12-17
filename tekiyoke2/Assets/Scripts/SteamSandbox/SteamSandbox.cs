using System;
using Ranking;
using ResultScene;
using Sirenix.OdinInspector;
using Steamworks;
using UnityEngine;

public class SteamSandbox : SerializedMonoBehaviour
{
    [SerializeField] IRankingSenderGetter rankingSenderGetter;
    
    [Button]
    void Send(RankKind kind, float time)
    {
        rankingSenderGetter.SendRanking(kind, time, () => print("sent!"));
    }
}
