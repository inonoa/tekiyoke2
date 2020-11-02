using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;

public class SakamichiSensors : MonoBehaviour
{
    [field: SerializeField, ReadOnly, LabelText("On Sakamichi L")]
    public bool OnSakamichiL{ get; private set; } = false;

    [field: SerializeField, ReadOnly, LabelText("On Sakamichi R")]
    public bool OnSakamichiR{ get; private set; } = false;

    [SerializeField] SakamichiSensor sensorL;
    [SerializeField] SakamichiSensor sensorR;

    [SerializeField] HeroMover hero;

    
    void Update()
    {
        sensorL.Update_(hero);
        sensorR.Update_(hero);

        OnSakamichiL = sensorL.IsTouching && !sensorR.IsTouching;
        OnSakamichiR = sensorR.IsTouching && !sensorL.IsTouching;
    }
}

[Serializable]
public class SakamichiSensor
{
    [field: SerializeField, ReadOnly, LabelText("Touching")]
    public bool IsTouching{ get; private set; } = false;

    [SerializeField] Vector2 rayOffset;
    [SerializeField] ContactFilter2D filter;

    RaycastHit2D[] hits = new RaycastHit2D[128];
    public void Update_(HeroMover hero)
    {
        Vector2 origin = hero.transform.position.ToVec2() + rayOffset;
        int num_hits = Physics2D.Raycast(origin, new Vector2(0, -1), filter, hits, 45);
        IsTouching = num_hits > 0;
    }
}
