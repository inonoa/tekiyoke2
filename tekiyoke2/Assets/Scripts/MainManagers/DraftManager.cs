﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DraftManager : MonoBehaviour
{
    public static DraftManager CurrentInstance{ get; private set; }

    [field: SerializeField, LabelText(nameof(StageIndex))]
    public int StageIndex { get; private set; } = -1;

    [Space(10), SerializeField] Transform _GameMasterTF;
    public Transform GameMasterTF => _GameMasterTF;
    [SerializeField] Transform _PauseMasterTF;
    public Transform PauseMasterTF => _PauseMasterTF;

    [SerializeField] HeroMover hero;

    [field: Space(30)]
    [SerializeField] StageDebugger debugger;
    
    void Awake()
    {
        CurrentInstance = this;
        Time.timeScale = 1;
    }

    void Start()
    {
        MemoryOverDeath memory = MemoryOverDeath.Instance;
        if(memory.HasData())
        {
            GameTimeCounter.CurrentInstance.Seconds  = memory.Time;
            GameTimeCounter.CurrentInstance.DoesTick = true;

            if(debugger != null && debugger.Debug && debugger.IgnoreMemory)
            {
                ApplyCheckPointData(debugger.CheckPoint.Index);
            }
            else
            {
                ApplyCheckPointData(memory.CheckPointIndex);
            }
        }
        else
        {
            GameTimeCounter.CurrentInstance.Seconds  = 0f;
            GameTimeCounter.CurrentInstance.DoesTick = true;

            if(debugger != null && debugger.Debug) ApplyCheckPointData(debugger.CheckPoint.Index);
            else                                   ApplyCheckPointData(-1);
        }
    }

    void ApplyCheckPointData(int index)
    {
        CheckPointsManager.Instance.Init(index);
        if(index != -1)
        {
            Vector2 respawnPos = CheckPointsManager.Instance.GetPosition(index);
            hero.WarpPos(respawnPos.x, respawnPos.y);
        }
    }
}
