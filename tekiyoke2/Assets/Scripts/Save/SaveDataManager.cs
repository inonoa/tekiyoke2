using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "Save Data Manager", menuName = "Scriptable Object/Save Data Manager")]
public class SaveDataManager : SerializedScriptableObject
{
    [SerializeField] SaveData data;
    
    public bool TutorialFinished => data.tutorialFinished;
    public IReadOnlyList<bool> StageCleared => data.stageCleared;
    public IReadOnlyList<float> BestTimes => data.bestTimes;
    

    [Space(10), SerializeField] IDataSaver saver;
    
    public void SetTutorialFinished()
    {
        data.tutorialFinished = true;
        Save();
    }

    public float SetStageData(StagePlayData play)
    {
        data.stageCleared[play.Stage] = true;

        float tmpBestTime = BestTimes[play.Stage];
        if (play.Time < BestTimes[play.Stage])
        {
            data.bestTimes[play.Stage] = play.Time;
        }

        Save();

        return tmpBestTime;
    }
    
    public void Init()
    {
        if (data == null) Load();
    }

    [Button]
    void Save() => saver.Save(data);
    
    [Button]
    void Load() => saver.Load(data => this.data = data);
}

public interface IDataSaver
{
    void Save(SaveData data);
    void Load(Action<SaveData> dataCallback);
}
