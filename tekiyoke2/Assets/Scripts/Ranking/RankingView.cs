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
        [Space(10)]
        [SerializeField] Transform bodyCenterTranform;
        [SerializeField] float bodyTiltTan = 2f;

        EnumArray<RankData, RankKind> rankDatas = new EnumArray<RankData, RankKind>();
        RankKind shownKind;

        public void SetData(RankData data)
        {
            rankDatas[data.Kind] = data;
            if (data.Kind == shownKind)
            {
                CreateNodes(data.Kind);
            }
        }

        void ClearNodes()
        {
            foreach (Transform node in top100Content)
            {
                Destroy(node.gameObject);
            }
            foreach (Transform node in aroundPlayer100Content)
            {
                Destroy(node.gameObject);
            }
        }

        void CreateNodes(RankKind kind)
        {
            foreach (RankDatum rankDatum in rankDatas[kind].Top100)
            {
                Instantiate(nodeViewPrefab, top100Content).Init(rankDatum, bodyCenterTranform, bodyTiltTan);
            }
            foreach (RankDatum rankDatum in rankDatas[kind].AroundPlayer100)
            {
                Instantiate(nodeViewPrefab, aroundPlayer100Content).Init(rankDatum, bodyCenterTranform, bodyTiltTan);
            }
        }

        public void Show(RankKind kind)
        {
            shownKind = kind;
            gameObject.SetActive(true);
            ClearNodes();
            
            if(rankDatas[kind] == null) return;
            CreateNodes(kind);
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