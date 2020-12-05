using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Ranking
{
    public class RankNodeView : MonoBehaviour
    {
        Transform centerTransform;
        Func<float> tiltTanGetter;
        [SerializeField] Text rankText;
        [SerializeField] Text nameText;
        [SerializeField] Text timeText;
        [SerializeField] Image bgImage;
        [SerializeField] Sprite normalSprite;
        [SerializeField] Sprite hilitSprite;

        public void Init(RankDatum datum, Transform centerTransform, Func<float> tiltTanGetter, Material mat)
        {
            rankText.text = datum.Rank.ToString();
            nameText.text = datum.Name;
            timeText.text = datum.Time.ToTimeString();
            
            this.centerTransform = centerTransform;
            this.tiltTanGetter = tiltTanGetter;
            this.bgImage.material = mat;
        }
        
        [Button]
        public void OnFocused()
        {
            bgImage.sprite = hilitSprite;
        }
        
        [Button]
        public void OnUnFocused()
        {
            bgImage.sprite = normalSprite;
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

        const string _DPAlpha = "_DPAlpha";
    }
}