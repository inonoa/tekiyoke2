using System;
using System.Linq;
using ResultScene;
using UniRx;
using UnityEngine;

namespace Ranking
{
    public class RankingLogger : MonoBehaviour, IRankView
    {
        RankData data;
        public void SetData(RankData data)
        {
            this.data = data;
        }

        public void Show(RankKind kind)
        {
            print("log!");
            print(string.Join("\n", data.Top100.Select(datum => datum.Name + ": " + datum.Time)));
            print(string.Join("\n", data.AroundPlayer100.Select(datum => datum.Name + ": " + datum.Time)));
            _OnExit.OnNext(Unit.Default);
        }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;
    }
}