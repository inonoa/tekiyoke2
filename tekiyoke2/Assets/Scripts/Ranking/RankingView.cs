using System;
using System.Collections.Generic;
using DG.Tweening;
using ResultScene;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Ranking
{
    public class RankingView : MonoBehaviour, IRankView
    {
        [SerializeField] UIFocusManager focusManager;
        [Space(10)]
        [SerializeField] Image BgImage;
        Color bgColor;
        [Space(10)]
        [SerializeField] CanvasGroup bodyGroup;
        [SerializeField] Transform top100Content;
        [SerializeField] RankingScrollViewController top100Controller;
        [SerializeField] Material top100NodesMat;
        [SerializeField] Transform aroundPlayer100Content;
        [SerializeField] RankingScrollViewController aroundPlayerController;
        [SerializeField] Material aroundPlayerNodesMat;
        [SerializeField] RankNodeView nodeViewPrefab;
        [Space(10)]
        [SerializeField] CanvasGroup leftGroup;
        [SerializeField] ExitButton exitButton;
        [Space(10)]
        [SerializeField] Transform bodyCenterTranform;
        [SerializeField] float bodyTiltTan = 2f;
        [SerializeField] float enterDuration = 0.5f;
        [SerializeField] float exitDuration = 0.5f;
        
        

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
        
        public void Show(RankKind kind)
        {
            shownKind = kind;
            gameObject.SetActive(true);
            focusManager.OnEnter();
            
            BgImage.color = bgColor * new Color(1, 1, 1, 0);
            bodyGroup.alpha = 0;
            bodyGroup.transform.SetLocalX(100);
            leftGroup.alpha = 0;
            leftGroup.transform.SetLocalX(-100);
            
            DOTween.Sequence()
                .Append(BgImage.DOColor(bgColor, enterDuration).SetEase(Ease.Linear))
                .Join(bodyGroup.DOFade(1, enterDuration).SetEase(Ease.Linear))
                .Join(bodyGroup.transform.DOLocalMoveX(0, enterDuration).SetEase(Ease.OutQuint))
                .Join(leftGroup.DOFade(1, enterDuration).SetEase(Ease.Linear))
                .Join(leftGroup.transform.DOLocalMoveX(0, enterDuration).SetEase(Ease.OutQuint));
            
            exitButton.OnEnter();
            
            ClearNodes();
            
            if(rankDatas[kind] == null) return;
            CreateNodes(kind);
        }

        void Exit()
        {
            DOTween.Sequence()
                .Append(BgImage.DOFade(0, exitDuration))
                .Join(bodyGroup.DOFade(0, exitDuration))
                .Join(bodyGroup.transform.DOLocalMoveX(100, exitDuration).SetEase(Ease.OutSine))
                .Join(leftGroup.DOFade(0, exitDuration))
                .Join(leftGroup.transform.DOLocalMoveX(-100, exitDuration).SetEase(Ease.OutSine))
                .AppendCallback(() => gameObject.SetActive(false));
            focusManager.OnExit();
            exitButton.OnExit();
        }
        
        void CreateNodes(RankKind kind)
        {
            List<RankNodeView> top100 = new List<RankNodeView>();
            foreach (RankDatum rankDatum in rankDatas[kind].Top100)
            {
                var node = Instantiate(nodeViewPrefab, top100Content);
                node.Init(rankDatum, bodyCenterTranform, () => bodyTiltTan, top100NodesMat);
                top100.Add(node);
            }
            top100Controller.OnNodesSet(top100);
            
            List<RankNodeView> aroundPlayer = new List<RankNodeView>();
            foreach (RankDatum rankDatum in rankDatas[kind].AroundPlayer100)
            {
                var node = Instantiate(nodeViewPrefab, aroundPlayer100Content);
                node.Init(rankDatum, bodyCenterTranform, () => bodyTiltTan, aroundPlayerNodesMat);
                aroundPlayer.Add(node);
            }
            aroundPlayerController.OnNodesSet(aroundPlayer);
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
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;

        void Awake()
        {
            exitButton.Pushed.Subscribe(_ =>
            {
                Exit();
                _OnExit.OnNext(Unit.Default);
            });
            bgColor = BgImage.color;
        }
    }
}