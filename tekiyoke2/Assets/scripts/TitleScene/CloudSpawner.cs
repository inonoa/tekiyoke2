using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CloudSpawner : MonoBehaviour
{
    public enum State
    {
        In, Active, Wind, Inactive
    }

    [ReadOnly] public State state = State.In;

    [SerializeField] List<GameObject> clouds2Spawn = new List<GameObject>();
    List<GameObject> cloudsExisting = new List<GameObject>();

    [SerializeField] GameObject AnyKey2Start;

    int countWhileActive = 0;
    int countWhileWind = 0;
    [SerializeField] int count2Title = 40;
    [SerializeField] int count2Spawn = 100;
    [SerializeField] float moveSpeed = 3;

    System.Random random = new System.Random();

    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            int idx2Spawn = random.Next(clouds2Spawn.Count);
            Vector3 position2Spawn = new Vector3(random.Next(-600, 600),random.Next(-400, 400),random.Next(-3, 0));
            cloudsExisting.Add(Instantiate(clouds2Spawn[idx2Spawn],position2Spawn,Quaternion.identity));
        }

        const float duration = 1f;

        DOVirtual.DelayedCall(0.5f + duration + 1, () => 
        {
            AnyKey2Start.GetComponent<SpriteRenderer>().DOFade(1, 0.35f).SetEase(Ease.OutQuint);
        });
    }

    void Update()
    {
        if(state==State.In || state==State.Active)
        {
            //追加
            countWhileActive ++;
            if(countWhileActive==count2Spawn)
            {
                countWhileActive = 0;
                int idx2Spawn = random.Next(clouds2Spawn.Count);
                Vector3 position2Spawn = new Vector3(random.Next(800,1000),random.Next(-500,500),random.Next(-3,0));
                cloudsExisting.Add(Instantiate(clouds2Spawn[idx2Spawn],position2Spawn,Quaternion.identity));
            }

            //移動、削除
            for(int i = cloudsExisting.Count - 1; i > -1; i--)
            {
                cloudsExisting[i].transform.position += new Vector3(-moveSpeed,0,0);
                if(cloudsExisting[i].transform.position.x < -800)
                {
                    Destroy(cloudsExisting[i]);
                    cloudsExisting.RemoveAt(i);
                }
            }
        }
        else if(state == State.Wind)
        {
            countWhileWind ++;
            if(countWhileWind==count2Title)
            {
                SceneTransition.Start2ChangeScene("StageChoiceScene",SceneTransition.TransitionType.Default);
            }
            for(int i = cloudsExisting.Count - 1; i > -1; i--)
            {
                cloudsExisting[i].transform.position += new Vector3(-moveSpeed*10,0,0);
                cloudsExisting[i].GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.03f);
                if(cloudsExisting[i].transform.position.x < -800)
                {
                    Destroy(cloudsExisting[i]);
                    cloudsExisting.RemoveAt(i);
                }
            }
            AnyKey2Start.transform.position += new Vector3(-moveSpeed*2,0,0);
            AnyKey2Start.GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.1f);
        }
    }
}

