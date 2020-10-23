using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryOverDeath
{
    public float Time{ get; private set; } = 0;
    public int CheckPointIndex{ get; private set; } = -1;

    public void PassCheckPoint(int index)
    {
        CheckPointIndex = index;
    }

    public void SaveOnDeath()
    {
        Time = GameTimeCounter.CurrentInstance.Seconds;
    }

    public void Clear()
    {
        Time            = 0;
        CheckPointIndex = -1;
    }

    public bool HasData()
    {
        return Time > 0;
    }


    #region Singleton
    static MemoryOverDeath _Instance;
    public static MemoryOverDeath Instance{
        get => (_Instance!=null ? _Instance : _Instance = new MemoryOverDeath());
    }
    private MemoryOverDeath(){ }
    #endregion
}
