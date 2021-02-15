using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Sirenix.OdinInspector;
using UniRx.Triggers;

public class CheckPoint : MonoBehaviour
{
    [field: SerializeField] [field: RenameField("Name")]
    public string Name{ get; private set; }
    [Button] void RenameObj() => gameObject.name = Name;

    public int Index { get; private set; } = -1;
    

    Subject<CheckPoint> _Passed = new Subject<CheckPoint>();
    public IObservable<CheckPoint> Passed => _Passed;

    public void Init(int index) => Index = index;

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(other => other.CompareTag(Tags.Hero))
            .Take(1)
            .Subscribe(_ => _Passed.OnNext(this));
    }
}

