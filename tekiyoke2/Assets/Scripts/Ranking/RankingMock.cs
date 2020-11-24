using System;
using ResultScene;
using UnityEngine;

namespace Ranking
{
    [CreateAssetMenu(fileName = "Ranking Mock", menuName = "Scriptable Object/Ranking Mock")]
    public class RankingMock : ScriptableObject, IRankingSenderGetter
    {
        [SerializeField] RankData data;

        public void SendRanking(RankKind kind, float time, Action onSent)
        {
            onSent.Invoke();
        }

        public void GetRanking(RankKind kind, Action<RankData> onGot)
        {
            onGot.Invoke(data);
        }
    }
}