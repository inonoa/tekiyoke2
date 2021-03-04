using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Config;

[CreateAssetMenu(fileName = "Save Data Manager", menuName = "Scriptable Object/Save Data Manager")]
public class SaveDataManager : SerializedScriptableObject, IPlayerNameChanger
{
    [SerializeField] SaveData _data;

    SaveData Data
    {
        get
        {
            if (_data == null) _data = dataSource.Copy();
            return _data;
        }
    }

    public string               PlayerName       => Data.playerName;
    public bool                 TutorialFinished => Data.tutorialFinished;
    public IReadOnlyList<bool>  StageCleared     => Data.stageCleared;
    public IReadOnlyList<float> BestTimes        => Data.bestTimes;
    public float                BGMVolume        => Data.bgmVolume;
    public float                SEVolume         => Data.seVolume;
    

    [SerializeField] IDataSaver saver;

    public void ChangePlayerName(string name)
    {
        Data.playerName = name;
        Save();
    }
    
    public void SetTutorialFinished()
    {
        Data.tutorialFinished = true;
        Save();
    }

    public (bool isFirstPlay, float lastBestTime) SetStageData(StagePlayData play)
    {
        bool tmpIsFirst = !StageCleared[play.Stage];
        Data.stageCleared[play.Stage] = true;

        float tmpBestTime = BestTimes[play.Stage];
        if (play.Time < BestTimes[play.Stage])
        {
            Data.bestTimes[play.Stage] = play.Time;
        }

        Save();

        return (tmpIsFirst, tmpBestTime);
    }

    public void SetBGMVolume(float volume)
    {
        Data.bgmVolume = volume;
        Save();
    }

    public void SetSEVolume(float volume)
    {
        Data.seVolume = volume;
        Save();
    }

    void Init()
    {
        if (Data == null) Load();
    }

    [Button]
    void Save() => saver.Save(Data);
    
    [Button]
    void Load() => saver.Load(data => this._data = data);
    
    [SerializeField] SaveData dataSource;
}

public interface IDataSaver
{
    void Save(SaveData data);
    void Load(Action<SaveData> dataCallback);
}
