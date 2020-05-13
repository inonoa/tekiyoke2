using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHolder
{
    readonly int numStages = 3;
    public float[] clearTimesLast;
    public float[] clearTimesBestExceptLast;

    public bool BestTimeExists(int index){
        return (index!=-1) ? (clearTimesBestExceptLast[index] >= 0) : false;
    }

    public void ApplyLastTime2Best(){
        for(int i=0;i<numStages;i++){
            if(clearTimesBestExceptLast[i] < 0 || clearTimesLast[i] < clearTimesBestExceptLast[i]){
                clearTimesBestExceptLast[i] = clearTimesLast[i];
            }
        }
    }


    #region Singleton
    static ScoreHolder _Instance;

    ///<summary>そもそも (アクセスしやすくしたいとしても) static classでよくない……？</summary>
    static public ScoreHolder Instance{
        get{
            if(_Instance == null) _Instance = new ScoreHolder();
            return _Instance;
        }
    }
    private ScoreHolder(){
        clearTimesLast = new float[numStages];
        clearTimesBestExceptLast = new float[numStages];
        for(int i=0;i<numStages;i++){
            clearTimesLast[i] = -1;
            clearTimesBestExceptLast[i] = -1;
        }
    }

    #endregion
}

public static class FloatToTimeExtension{
    
    public static string ToTimeString(this float seconds){

        int csc = (int)((seconds - (int)seconds) * 100);
        int sec = ((int)seconds)%60;
        int min = ((int)seconds) / 60;

        return min.ToString("00") + ":" + sec.ToString("00") + ":" + csc.ToString("00");
    }
}
