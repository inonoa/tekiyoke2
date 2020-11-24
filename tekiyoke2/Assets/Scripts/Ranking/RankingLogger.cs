using System.Linq;
using ResultScene;
using UnityEngine;

namespace Ranking
{
    public class RankingLogger : MonoBehaviour, IRankView
    {
        public void Show(RankData rankData)
        {
            print("log!");
            print(string.Join("\n", rankData.Top100.Select(datum => datum.Name + ": " + datum.Time)));
            print(string.Join("\n", rankData.AroundPlayer100.Select(datum => datum.Name + ": " + datum.Time)));
        }
    }
}