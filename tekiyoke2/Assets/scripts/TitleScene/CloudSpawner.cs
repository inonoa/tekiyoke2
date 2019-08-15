using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CloudSpawner : MonoBehaviour
{
    public enum State{
        Active, Wind, Inactive
    }

    public State state = State.Active;

    public List<GameObject> clouds2Spawn = new List<GameObject>();
    private List<GameObject> cloudsExisting = new List<GameObject>();

    private int count = 0;
    public int count2Spawn = 100;
    public int moveSpeed = 3;

    private System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state==State.Active){
            //追加
            count ++;
            if(count==count2Spawn){
                count = 0;
                int idx2Spawn = random.Next(clouds2Spawn.Count);
                Vector3 position2Spawn = new Vector3(random.Next(800,1000),random.Next(-500,500),random.Next(-3,0));
                cloudsExisting.Add(Instantiate(clouds2Spawn[idx2Spawn],position2Spawn,Quaternion.identity));
            }

            //移動、削除
            for(int i=cloudsExisting.Count-1;i>-1;i--){
                cloudsExisting[i].transform.position += new Vector3(-moveSpeed,0,0);
                if(cloudsExisting[i].transform.position.x < -800){
                    Destroy(cloudsExisting[i]);
                    cloudsExisting.RemoveAt(i);
                }
            }
        }
    }
}
