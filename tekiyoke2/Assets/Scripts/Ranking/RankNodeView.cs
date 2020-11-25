using UnityEngine;
using UnityEngine.UI;

namespace Ranking
{
    public class RankNodeView : MonoBehaviour
    {
        [SerializeField] Text rankText;
        [SerializeField] Text nameText;
        [SerializeField] Text timeText;
        
        public void Set(RankDatum datum)
        {
            rankText.text = datum.Rank.ToString();
            nameText.text = datum.Name;
            timeText.text = datum.Time.ToTimeString();
        }
    }
}