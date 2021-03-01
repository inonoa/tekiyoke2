using System;
using Config;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class StageSelectManager : SerializedMonoBehaviour
{
    [SerializeField] IStageSelectView view;
    [SerializeField] ConfigManager configManager;
    [SerializeField] RankingsSelectManager rankingsSelectManager;

    void Start()
    {
        view.StageSelected.Subscribe(stage =>
        {
            SceneTransition.StartToChangeScene<NormalTransitionView>(stage.ToString());
        });
        view.OnGoToConfig.Subscribe(_ => configManager.Enter());
        view.OnGoToRankings.Subscribe(_ => rankingsSelectManager.Enter());

        configManager.OnExit.Subscribe(_ => view.Enter());
        rankingsSelectManager.OnExit.Subscribe(_ => view.Enter());
        
        view.Enter();
    }
}

public interface IStageSelectView
{
    void Enter();
    IObservable<EDraft> StageSelected { get; }
    IObservable<Unit> OnGoToConfig { get; }
    IObservable<Unit> OnGoToRankings { get; }
}
