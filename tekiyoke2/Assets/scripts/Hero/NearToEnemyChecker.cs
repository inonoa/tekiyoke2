using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearToEnemyChecker : MonoBehaviour
{
    static readonly float timeScaleWhenNear = 0.3f;

    [SerializeField]
    bool slowNearEnemy = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Enemy" & slowNearEnemy){
            Tokitome.SetTime(timeScaleWhenNear);
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag=="Enemy" && slowNearEnemy){
            Tokitome.SetTime(1);
        }
    }
}
