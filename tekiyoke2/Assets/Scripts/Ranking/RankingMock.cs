using System;
using ResultScene;
using UnityEngine;

namespace Ranking
{
    [CreateAssetMenu(fileName = "Ranking Mock", menuName = "Scriptable Object/Ranking Mock")]
    public class RankingMock : ScriptableObject, IRankingSenderGetter
    {
        [SerializeField] RankData data;

        public void SendGetRanking(RankKind kind, float time, Action<RankData> onGot)
        {
            onGot.Invoke(data);
        }
    }
}