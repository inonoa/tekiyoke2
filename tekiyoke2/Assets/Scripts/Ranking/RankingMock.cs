using System;
using System.Linq;
using System.Runtime.InteropServices;
using ResultScene;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

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

        [Button]
        void RandomGenerate(RankKind kind, int lengthTop, int lengthAroundPlayer)
        {
            var allTimes = RandomUtil
                           .Generate(() => Random.Range(20f, 100f), lengthTop + lengthAroundPlayer)
                           .OrderBy(time => time);

            RankDatum[] top = allTimes
                              .Take(lengthTop)
                              .Select((time, i) => new RankDatum(RandName(), i + 1, time))
                              .ToArray();
            
            RankDatum[] aroundYou = allTimes
                                    .Skip(lengthTop)
                                    .Select((time, i) => new RankDatum(RandName(), lengthTop + 67 + i + 1, time)) //67は適当
                                    .ToArray();
            
            datas[(int)kind] = new RankData(kind, top, aroundYou);
        }

        string RandName(int lenMin = 3, int lenMax = 8)
        {
            char[] possibleCharacters = ("abcdefghijklmnopqrstuvwxyz" 
                                       + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                       + "01234567890")
                                       .ToCharArray();
            int len = Random.Range(lenMin, lenMax + 1);
            
            return new string(RandomUtil.Generate(() => possibleCharacters.RandomPick(), len).ToArray());
        }
    }
}