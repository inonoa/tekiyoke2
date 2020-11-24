using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Save Data", menuName = "Scriptable Object/Save Data")]
public class SaveData : ScriptableObject
{
    public bool    tutorialFinished = false;
    public bool[]  stageCleared     = new bool[3];
    public float[] bestTimes        = new float[3];

    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>()
        {
            {nameof(tutorialFinished), tutorialFinished.ToString()},
            {nameof(stageCleared), string.Join(",", stageCleared)},
            {nameof(bestTimes), string.Join(",", bestTimes)}
        };
    }

    public static SaveData FromDictionary(Dictionary<string, string> dict)
    {
        var data = ScriptableObject.CreateInstance<SaveData>();
        
        data.tutorialFinished = bool.Parse(dict[nameof(tutorialFinished)]);
        data.stageCleared     = dict[nameof(stageCleared)]
                                .Split(',')
                                .Select(bool.Parse)
                                .ToArray();
        data.bestTimes        = dict[nameof(bestTimes)]
                                .Split(',')
                                .Select(float.Parse)
                                .ToArray();

        return data;
    }

    public SaveData Copy()
    {
        SaveData copy = ScriptableObject.CreateInstance<SaveData>();
        
        copy.tutorialFinished = this.tutorialFinished;
        copy.stageCleared = new bool[3];
        this.stageCleared.CopyTo(copy.stageCleared, 0);
        copy.bestTimes = new float[3];
        this.bestTimes.CopyTo(copy.bestTimes, 0);
        
        return copy;
    }
}
