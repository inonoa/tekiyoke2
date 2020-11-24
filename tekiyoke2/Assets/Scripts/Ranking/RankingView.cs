using System;
using ResultScene;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Ranking
{
    public class RankingView : MonoBehaviour, IRankView
    {
        [SerializeField] Transform top100Content;
        [SerializeField] Transform aroundPlayer100Content;
        [SerializeField] RankNodeView nodeViewPrefab;
        [SerializeField] Button exitButton;

        public void SetData(RankData data)
        {
            foreach (RankDatum rankDatum in data.Top100)
            {
                Instantiate(nodeViewPrefab, top100Content).Set(rankDatum);
            }
            foreach (RankDatum rankDatum in data.AroundPlayer100)
            {
                Instantiate(nodeViewPrefab, aroundPlayer100Content).Set(rankDatum);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;

        void Start()
        {
            exitButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                _OnExit.OnNext(Unit.Default);
            });
        }
    }
}