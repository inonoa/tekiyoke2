using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ranking
{
    public class RankNodeView : MonoBehaviour
    {
        Transform centerTransform;
        Func<float> tiltTanGetter;
        [SerializeField] Text rankText;
        [SerializeField] Text nameText;
        [SerializeField] Text timeText;

        public void Init(RankDatum datum, Transform centerTransform, Func<float> tiltTanGetter)
        {
            rankText.text = datum.Rank.ToString();
            nameText.text = datum.Name;
            timeText.text = datum.Time.ToTimeString();
            
            this.centerTransform = centerTransform;
            this.tiltTanGetter = tiltTanGetter;
        }

        void Update()
        {
            float relativeY = transform.position.y - centerTransform.position.y;
            float targetX = centerTransform.position.x + relativeY / tiltTanGetter.Invoke();
            transform.SetX(targetX);
        }

        new Transform transform;
        void Awake()
        {
            this.transform = base.transform;
        }
    }
}