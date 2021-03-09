using System;
using System.Collections.Generic;
using Config;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class StageSelectManager : SerializedMonoBehaviour
{
    [SerializeField] IStageSelectView view;
    [SerializeField] ConfigManager configManager;
    [SerializeField] RankingsSelectManager rankingsSelectManager;
    [SerializeField] SaveDataManager saveDataManager;

    void Start()
    {
        view.StageSelected.Subscribe(stage =>
        {
            SceneTransition.StartToChangeScene<NormalTransitionView>(stage.ToString());
        });
        view.OnGoToConfig.Subscribe(_ => configManager.Enter());
        view.OnGoToRankings.Subscribe(_ => RankingsEnter());

        configManager.OnExit.Subscribe(_ => ViewEnter());
        rankingsSelectManager.OnExit.Subscribe(_ => ViewEnter());
        
        ViewEnter();
        
        if (saveDataManager.StageIsBeingUnlocked)
        {
            saveDataManager.SetStageBeingUnlocked(false);
        }
    }

    void ViewEnter()
    {
        bool[] selectables =
        {
            true,
            saveDataManager.StageCleared[0],
            saveDataManager.StageCleared[1]
        };
        bool unlocking = saveDataManager.StageIsBeingUnlocked;
        
        view.Enter(selectables, unlocking);
    }

    void RankingsEnter()
    {
        rankingsSelectManager.Enter(saveDataManager.StageCleared);
    }
}

public interface IStageSelectView
{
    void Enter(IReadOnlyList<bool> draftsSelectable, bool unlocking);
    IObservable<EDraft> StageSelected { get; }
    IObservable<Unit> OnGoToConfig { get; }
    IObservable<Unit> OnGoToRankings { get; }
}
