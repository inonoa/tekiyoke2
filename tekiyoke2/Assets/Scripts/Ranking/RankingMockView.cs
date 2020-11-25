using System;
using ResultScene;
using UniRx;
using Unity.Collections;
using UnityEngine;

namespace Ranking
{
    [CreateAssetMenu(menuName = "Scriptable Object/Ranking Mock View")]
    public class RankingMockView : ScriptableObject, IRankView
    {
        [SerializeField, ReadOnly] RankData[] datas = new RankData[Enum.GetValues(typeof(RankKind)).Length];
        [SerializeField] RankData current;

        public void SetData(RankData data)
        {
            datas[(int)data.Kind] = data;
        }

        public void Show(RankKind kind)
        {
            current = datas[(int) kind];
        }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;
    }
}
