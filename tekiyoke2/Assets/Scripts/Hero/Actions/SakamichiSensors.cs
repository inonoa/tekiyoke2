using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SakamichiSensors : MonoBehaviour
{
    [SerializeField] ContactFilter2D filter;

    [field: SerializeField, ReadOnly, LabelText("L")]
    public bool L{ get; private set; } = false;

    [field: SerializeField, ReadOnly, LabelText("R")]
    public bool R{ get; private set; } = false;

    [SerializeField, ReadOnly] bool touchedL = true;
    [SerializeField, ReadOnly] bool touchedR = true;

    [SerializeField] Collider2D triggerL;
    [SerializeField] Collider2D triggerR;

    
    void Update()
    {
        touchedL = triggerL.IsTouching(filter);
        touchedR = triggerR.IsTouching(filter);

        L = touchedL && !touchedR;
        R = touchedR && !touchedL;
    }
}
