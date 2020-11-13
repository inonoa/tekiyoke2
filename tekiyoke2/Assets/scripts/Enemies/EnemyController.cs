using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyController : MonoBehaviour, ISpawnsNearHero
{
    [SerializeField] DPinEnemy dpcd;
    public DPinEnemy DPCD => dpcd;

    protected TimeManager TimeManager{ get; private set; }

    protected void Init()
    {
        TimeManager = TimeManager.CurrentInstance;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        OnSpawned();
    }
    protected virtual void OnSpawned(){ }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
