using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TogeDropRespawner : MonoBehaviour
{

    public void AddDrop(TogeDropController drop){
        drops2count[drop.gameObject] = -1;
    }
    public void SendDeath(TogeDropController drop, int frames2Respawn){
        drops2count[drop.gameObject] = frames2Respawn;
    }
    
    Dictionary<GameObject, int> drops2count = new Dictionary<GameObject, int>();

    void Update()
    {
        var keys = drops2count.Keys.ToList();
        foreach(GameObject key in keys){
            drops2count[key] --; //なんか変？
            if(drops2count[key] == 0){
                key.SetActive(true);
                key.transform.position = key.GetComponent<TogeDropController>().DefaultPosition;
                key.GetComponent<TogeDropController>().OnRespawn();
            }
        }
    }

    static public TogeDropRespawner Instance{ get; private set; }
    void Awake() { Instance = this; }
}
