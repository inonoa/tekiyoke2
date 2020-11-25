using System;
using System.Linq;
using ResultScene;
using UnityEngine;

namespace Ranking
{
    [CreateAssetMenu(fileName = "Ranking Mock", menuName = "Scriptable Object/Ranking Mock")]
    public class RankingMock : ScriptableObject, IRankingSenderGetter
    {
        [SerializeField] RankData[] datas = new RankData[Enum.GetValues(typeof(RankKind)).Length];

        public void SendRanking(RankKind kind, float time, Action onSent)
        {
            onSent.Invoke();
        }

        public void GetRanking(RankKind kind, Action<RankData> onGot)
        {
            onGot.Invoke(datas.FirstOrDefault(data => data.Kind == kind));
        }
    }
}