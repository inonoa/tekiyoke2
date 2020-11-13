using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public abstract class EnemyController : MonoBehaviour, ISpawnsNearHero
{
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
