using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class StageDebugger : MonoBehaviour
{
    [SerializeField] bool _Debug = false;
    public bool Debug => _Debug;
    
    [SerializeField] CheckPoint _CheckPoint;
    public CheckPoint CheckPoint => _CheckPoint;
    
    [SerializeField] bool _IgnoreMemory = false;
    public bool IgnoreMemory => _IgnoreMemory;
    
    [PropertySpace(10)]
    [Button]
    void WarpToCheckPoint(CheckPoint checkPoint)
    {
        Vector2 respawnPos = checkPoint.transform.position;
        hero.WarpPos(respawnPos.x, respawnPos.y);
    }
    
    [Space(10)]
    [SerializeField] HeroMover hero;
}