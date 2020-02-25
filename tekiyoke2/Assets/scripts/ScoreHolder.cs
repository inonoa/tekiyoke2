using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHolder
{
    int numStages = 3;
    public float[] clearTimesLast;
    public float[] clearTimesBest;


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
        clearTimesBest = new float[numStages];
    }
    
    #endregion
}
