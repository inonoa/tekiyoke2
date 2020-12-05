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
        [SerializeField] RankingScrollViewController top100Controller;
        [SerializeField] RankingScrollViewController aroundPlayerController;
        [Space(10)]
        [SerializeField] CanvasGroup leftGroup;
        [SerializeField] ExitButton exitButton;
        [Space(10)]
        [SerializeField] float enterDuration = 0.5f;
        [SerializeField] float exitDuration = 0.5f;

        readonly ReactiveCollection<RankData> rankDatas = new ReactiveCollection<RankData>
        (
            new RankData[Enum.GetValues(typeof(RankKind)).Length]
        );
        readonly ReactiveProperty<RankKind> shownKind = new ReactiveProperty<RankKind>();

        public void SetData(RankData data)
        {
            if(!initialized) Init();
            
            rankDatas[(int) data.Kind] = data;
        }
        
        public void Show(RankKind kind)
        {
            if(!initialized) Init();
            
            shownKind.Value = kind;
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
            top100Controller.OnExit();
            aroundPlayerController.OnExit();
        }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;

        bool initialized;
        void Init()
        {
            initialized = true;
            
            exitButton.Pushed.Subscribe(_ =>
            {
                Exit();
                _OnExit.OnNext(Unit.Default);
            });
            bgColor = BgImage.color;

            var shownDataChanged = rankDatas
                .ObserveReplace()
                .Where(replace => replace.Index == (int) shownKind.Value)
                .CombineLatest(shownKind, (replace, shown) => rankDatas[(int)shown]);

            shownDataChanged.Subscribe(data => print(data.Kind));
            
            top100Controller
                .Init(shownDataChanged.Select(data => data.Top100));
            aroundPlayerController
                .Init(shownDataChanged.Select(data => data.AroundPlayer100));
        }
    }
}