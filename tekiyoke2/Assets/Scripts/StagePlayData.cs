using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class StagePlayData
{
    [field: SerializeField, LabelText(nameof(Stage))]
    public int   Stage { get; private set; }
    [field: SerializeField, LabelText(nameof(Time))]
    public float Time  { get; private set; }

    public StagePlayData(int stage, float time)
    {
        this.Stage = stage;
        this.Time  = time;
    }
}
