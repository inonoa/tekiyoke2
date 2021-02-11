using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;


public class EnemySpawner : MonoBehaviour
{
    ISpawnsNearHero[] enemies;

    bool spawnedYet = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(spawnedYet) return;

        if(other.CompareTag("CanSeeEnemy"))
        {
            foreach(ISpawnsNearHero enemy in enemies)
            {
                enemy.Spawn();
            }
            spawnedYet = true;
        }
    }

    void Start()
    {
        enemies = GetComponentsInChildren<ISpawnsNearHero>(includeInactive: true);
        foreach(ISpawnsNearHero enemy in enemies) enemy.Hide();
    }

    [Button]
    void SetEnemiesDP(int dp)
    {
#if UNITY_EDITOR
        GetComponentsInChildren<IHaveDPinEnemy>(includeInactive: true)
            .Select(enemy => enemy.DPCD)
            .ForEach(dpcd =>
            {
                Undo.RecordObject(dpcd, "Set DP From Script");
                dpcd.ForceSetDP(dp);
                EditorUtility.SetDirty(dpcd);
            });
#endif
    }
}

public interface ISpawnsNearHero
{
    void Spawn();
    void Hide();
}
