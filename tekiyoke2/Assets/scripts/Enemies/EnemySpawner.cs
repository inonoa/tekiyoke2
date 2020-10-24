using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    EnemyController[] enemies;

    Vector3[] defaultPositions;

    bool spawnedYet = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(spawnedYet) return;

        if(other.gameObject.tag=="CanSeeEnemy")
        {
            foreach(EnemyController enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
                enemy.OnSpawned();
            }
            spawnedYet = true;
        }
    }

    void Start()
    {
        enemies = GetComponentsInChildren<EnemyController>(true);
        defaultPositions = enemies.Select(e => e.transform.position).ToArray();
        foreach(EnemyController enemy in enemies) enemy.gameObject.SetActive(false);
    }
}
