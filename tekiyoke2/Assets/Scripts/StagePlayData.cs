using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePlayData
{
    public readonly int   Stage;
    public readonly float Time;

    public StagePlayData(int stage, float time)
    {
        this.Stage = stage;
        this.Time  = time;
    }
}
