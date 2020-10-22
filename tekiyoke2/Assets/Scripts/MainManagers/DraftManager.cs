using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftManager : MonoBehaviour
{
    public static DraftManager CurrentInstance{ get; private set; }

    [SerializeField] Transform _GameMasterTF;
    public Transform GameMasterTF => _GameMasterTF;
    [SerializeField] Transform _PauseMasterTF;
    public Transform PauseMasterTF => _PauseMasterTF;

    [SerializeField] HeroMover hero;
    
    void Awake()
    {
        CurrentInstance = this;
        Tokitome.SetTime(1); //ここなんかなあ
    }

    void Start()
    {
        MemoryOverDeath memory = MemoryOverDeath.Instance;
        if(memory.HasData())
        {
            GameTimeCounter.CurrentInstance.Seconds  = memory.Time;
            GameTimeCounter.CurrentInstance.DoesTick = true;

            CheckPointsManager.Instance.Init(memory.CheckPointIndex);
            if(memory.CheckPointIndex != -1)
            {
                Vector2 respawnPos = CheckPointsManager.Instance.GetPosition(memory.CheckPointIndex);
                hero.WarpPos(respawnPos.x, respawnPos.y);
            }
        }
        else
        {
            GameTimeCounter.CurrentInstance.Seconds  = 0f;
            GameTimeCounter.CurrentInstance.DoesTick = true;

            CheckPointsManager.Instance.Init(frontLineIndex: -1);
        }
    }
}
