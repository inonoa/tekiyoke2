﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine.Serialization;
using Random = System.Random;

public class CloudSpawner : MonoBehaviour
{
    enum State{ Active, FadingOut }

    State state = State.Active;

    [SerializeField] List<GameObject> cloudPrefabsToSpawn = new List<GameObject>();
    
    [SerializeField, ReadOnly] List<GameObject> cloudsExisting;

    [SerializeField] AnyKeyToStart anyKeyToStart;
    [SerializeField] int numInitialClouds = 10;
    [SerializeField] float fadeOutToTitle = 1f;
    [SerializeField] float spawnIntervalSeconds = 2;
    [SerializeField] float moveSpeed = 200;

    [Space(10)] [SerializeField] float anyKeyToStartDelay = 2.5f;


    void Start()
    {
        cloudsExisting = new List<GameObject>();
        foreach (int _ in Enumerable.Range(0, numInitialClouds))
        {
            cloudsExisting.Add(SpawnCloud(canSpawnInScreen: true));
        }

        Observable.Interval(TimeSpan.FromSeconds(spawnIntervalSeconds))
            .Subscribe(_ => cloudsExisting.Add(SpawnCloud(canSpawnInScreen: false)));

        DOVirtual.DelayedCall(anyKeyToStartDelay, () => anyKeyToStart.Appear());
    }

    System.Random random = new System.Random();
    GameObject SpawnCloud(bool canSpawnInScreen)
    {
        int idx2Spawn = random.Next(cloudPrefabsToSpawn.Count);
        Vector3 position2Spawn = new Vector3
        (
            canSpawnInScreen ? random.Next(-600, 600) : random.Next(800,1000),
            random.Next(-500, 500),
            random.Next(-3, 0)
        );
        return Instantiate(cloudPrefabsToSpawn[idx2Spawn], position2Spawn, Quaternion.identity);
    }
    
    public void FadeOut()
    {
        state = State.FadingOut;
        anyKeyToStart.Disappear();

        foreach (var cloud in cloudsExisting)
        {
            cloud.transform.DOLocalMoveX(-500, fadeOutToTitle).SetRelative();
            cloud.GetComponent<SpriteRenderer>().DOFade(0, fadeOutToTitle);
        }

        DOVirtual.DelayedCall(fadeOutToTitle, () =>
        {
            SceneTransition.Start2ChangeScene("StageChoiceScene", SceneTransition.TransitionType.Default);
        });
    }

    void Update()
    {
        if(state == State.FadingOut) return;
        
        foreach (var cloud in cloudsExisting)
        {
            cloud.transform.position += new Vector3(- moveSpeed * Time.deltaTime, 0, 0);
        }
        cloudsExisting.Where(CloudIsGone).ForEach(Destroy);
        cloudsExisting.RemoveAll(CloudIsGone);
    }

    bool CloudIsGone(GameObject cloud)
    {
        return cloud.transform.position.x < -800;
    }
}

