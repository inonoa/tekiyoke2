using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
}

public interface ISpawnsNearHero
{
    void Spawn();
    void Hide();
}
