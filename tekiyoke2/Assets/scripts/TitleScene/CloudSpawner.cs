using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CloudSpawner : MonoBehaviour
{
    public enum State{
        Active, Wind, Inactive
    }

    public State state = State.Active;

    public List<GameObject> clouds2Spawn = new List<GameObject>();
    private List<GameObject> cloudsExisting = new List<GameObject>();

    public GameObject title;
    public GameObject AnyKey2Start;

    private int countWhileActive = 0;
    private int countWhileWind = 0;
    public int count2Title = 40;
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
            countWhileActive ++;
            if(countWhileActive==count2Spawn){
                countWhileActive = 0;
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

        }else if(state==State.Wind){
            countWhileWind ++;
            if(countWhileWind==count2Title){
                SceneManager.LoadScene("StageChoiceScene");
            }
            for(int i=cloudsExisting.Count-1;i>-1;i--){
                cloudsExisting[i].transform.position += new Vector3(-moveSpeed*10,0,0);
                cloudsExisting[i].GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.03f);
                if(cloudsExisting[i].transform.position.x < -800){
                    Destroy(cloudsExisting[i]);
                    cloudsExisting.RemoveAt(i);
                }
            }
            title.transform.position += new Vector3(-moveSpeed*2,0,0);
            title.GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.1f);
            AnyKey2Start.transform.position += new Vector3(-moveSpeed*2,0,0);
            AnyKey2Start.GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.1f);
        }
    }
}
