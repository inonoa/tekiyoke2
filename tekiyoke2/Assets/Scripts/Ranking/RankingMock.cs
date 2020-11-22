using System;
using ResultScene;
using UnityEngine;

namespace Ranking
{
    [CreateAssetMenu(fileName = "Ranking Mock", menuName = "Scriptable Object/Ranking Mock")]
    public class RankingMock : ScriptableObject, IRankingSenderGetter
    {
        [SerializeField] RankData data;

        public void SendGetRanking(float time, Action<RankData> onGot)
        {
            Debug.Log(time + " is sent!");
            onGot.Invoke(data);
        }
    }
}