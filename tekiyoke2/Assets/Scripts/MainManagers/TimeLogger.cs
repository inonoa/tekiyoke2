using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TimeLogger : MonoBehaviour
{
    [SerializeField] TimeManager manager;

    [Button]
    void Log()
    {
        print($"Hero:\n DeltaTime: {manager.DeltaTimeAroundHero}\n fixedDeltaTime: {manager.FixedDeltaTimeAroundHero}\n\n"
            + $"Others:\n DeltaTime: {manager.DeltaTimeExceptHero}\n fixedDeltaTime: {manager.FixedDeltaTimeExceptHero}\n");
    }
}
