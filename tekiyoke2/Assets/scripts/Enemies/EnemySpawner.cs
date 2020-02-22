using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    EnemyController[] enemies;

    Vector3[] defaultPositions;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="CanSeeEnemy"){
            foreach(EnemyController enemy in enemies) enemy.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemies = GetComponentsInChildren<EnemyController>(true);
        defaultPositions = enemies.Select(e => e.transform.position).ToArray();
    }
}
