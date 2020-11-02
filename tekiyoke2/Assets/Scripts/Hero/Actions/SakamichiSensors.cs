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

    [SerializeField] float width = 20;

    [SerializeField] HeroMover hero;

    
    void Update()
    {
        Vector3 heroFoot = hero.transform.position + new Vector3(0, -48);
        Vector2 down     = Vector2.down;
        float   rayLen   = 50;
        int     terrain  = LayerMask.GetMask("Terrain");

        RaycastHit2D hitL = Physics2D.Raycast(heroFoot - new Vector3(width / 2, 0), down, rayLen, terrain);
        RaycastHit2D hitR = Physics2D.Raycast(heroFoot + new Vector3(width / 2, 0), down, rayLen, terrain);

        float normalL = Vector2.Angle(Vector2.right, hitL.normal);
        float normalR = Vector2.Angle(Vector2.right, hitR.normal);

        OnSakamichiL = normalL.In(10,  80)  || normalR.In(10,  80);
        OnSakamichiR = normalL.In(100, 170) || normalR.In(100, 170);
    }
}
