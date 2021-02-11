using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DraftModeParameters", menuName = "Scriptable Object/Draft Mode Parameters", order = 100)]
public class DraftModeParams : ScriptableObject
{
    [SerializeField] float _TimeScale   = 0.2f;
    [SerializeField] float _DpPerSecond = 3;
    [SerializeField] float _GotDpRate   = 3;
    [SerializeField] bool _Muteki = false;

    public float TimeScale   => _TimeScale;
    public float DpPerSecond => _DpPerSecond;
    public float GotDpRate   => _GotDpRate;
    public bool Muteki => _Muteki;
}