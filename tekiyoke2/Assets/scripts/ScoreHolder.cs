using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score Holder", menuName = "Scriptable Object/Score Holder")]
public class ScoreHolder : ScriptableObject
{
    [SerializeField] StagePlayData data;
    public void Set(StagePlayData data) => this.data = data;
    public StagePlayData Get() => data;
}

public static class FloatToTimeExtension
{
    public static string ToTimeString(this float seconds){

        int csc = (int)((seconds - (int)seconds) * 100);
        int sec = ((int)seconds)%60;
        int min = ((int)seconds) / 60;

        return min.ToString("00") + ":" + sec.ToString("00") + ":" + csc.ToString("00");
    }
}
