using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryOverDeath
{
    public float time;
    public string checkPoint;

    public void Save(){
        time = GameTimeCounter.CurrentInstance.count;
        checkPoint = "とりあえず";
    }

    public void Load(){
        GameTimeCounter.CurrentInstance.count = time;
    }


    #region Singleton
    static MemoryOverDeath _Instance;
    public static MemoryOverDeath Instance{
        get => (_Instance!=null ? _Instance : _Instance = new MemoryOverDeath());
    }
    private MemoryOverDeath(){ }
    #endregion
}
