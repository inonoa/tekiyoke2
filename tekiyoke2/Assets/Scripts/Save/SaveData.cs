﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "Save Data", menuName = "Scriptable Object/Save Data")]
public class SaveData : ScriptableObject
{
    public bool    playerNameInitiallized = false;
    public string  playerName             = "";
    public bool    tutorialFinished       = false;
    public bool[]  stageCleared           = new bool[3];
    public bool    stageBeingUnlocked     = false;
    public float[] bestTimes              = new float[3]{ float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity };
    [Range(0, 1)]
    public float   bgmVolume              = 0.8f;
    [Range(0, 1)]
    public float   seVolume               = 0.8f;

    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>()
        {
            {nameof(playerNameInitiallized), playerNameInitiallized.ToString()},
            {nameof(playerName),             playerName},
            {nameof(tutorialFinished),       tutorialFinished.ToString()},
            {nameof(stageCleared),           string.Join(",", stageCleared)},
            {nameof(stageBeingUnlocked),     stageBeingUnlocked.ToString()},
            {nameof(bestTimes),              string.Join(",", bestTimes)},
            {nameof(bgmVolume),              bgmVolume.ToString()},
            {nameof(seVolume),               seVolume.ToString()}
        };
    }

    public static SaveData FromDictionary(Dictionary<string, string> dict)
    {
        var data = ScriptableObject.CreateInstance<SaveData>();

        data.playerNameInitiallized = bool.Parse(dict[nameof(playerNameInitiallized)]);
        data.playerName             = dict[nameof(playerName)];
        data.tutorialFinished       = bool.Parse(dict[nameof(tutorialFinished)]);
        data.stageCleared           = dict[nameof(stageCleared)]
                                      .Split(',')
                                      .Select(bool.Parse)
                                      .ToArray();
        data.stageBeingUnlocked     = bool.Parse(dict[nameof(stageBeingUnlocked)]);
        data.bestTimes              = dict[nameof(bestTimes)]
                                      .Split(',')
                                      .Select(float.Parse)
                                      .ToArray();
        data.bgmVolume              = float.Parse(dict[nameof(bgmVolume)]);
        data.seVolume               = float.Parse(dict[nameof(seVolume)]);

        return data;
    }

    public SaveData Copy()
    {
        SaveData copy = ScriptableObject.CreateInstance<SaveData>();

        copy.playerNameInitiallized = this.playerNameInitiallized;
        copy.playerName = this.playerName;
        copy.tutorialFinished = this.tutorialFinished;
        copy.stageCleared = new bool[3];
        this.stageCleared.CopyTo(copy.stageCleared, 0);
        copy.stageBeingUnlocked = this.stageBeingUnlocked;
        copy.bestTimes = new float[3];
        this.bestTimes.CopyTo(copy.bestTimes, 0);
        copy.bgmVolume = this.bgmVolume;
        copy.seVolume = this.seVolume;
        
        return copy;
    }
    
    [Button] void resetbesttime() => bestTimes = new float[3]{ float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity };
}
