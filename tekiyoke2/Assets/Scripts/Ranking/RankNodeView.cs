using UnityEngine;
using UnityEngine.UI;

namespace Ranking
{
    public class RankNodeView : MonoBehaviour
    {
        Transform centerTransform;
        float tiltTan;
        [SerializeField] Text rankText;
        [SerializeField] Text nameText;
        [SerializeField] Text timeText;

        public void Init(RankDatum datum, Transform centerTransform, float tiltTan)
        {
            rankText.text = datum.Rank.ToString();
            nameText.text = datum.Name;
            timeText.text = datum.Time.ToTimeString();
            
            this.centerTransform = centerTransform;
            this.tiltTan = tiltTan;
        }

        void Update()
        {
            float relativeY = transform.position.y - centerTransform.position.y;
            float targetX = centerTransform.position.x + relativeY / tiltTan;
            transform.SetX(targetX);
        }
    }
}