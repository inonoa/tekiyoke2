using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Sirenix.OdinInspector;

public class CheckPoint : MonoBehaviour
{
    [field: SerializeField] [field: RenameField("Name")]
    public string Name{ get; private set; }
    [Button] void RenameObj() => gameObject.name = Name;

    public int Index { get; private set; } = -1;
    

    Subject<CheckPoint> _Passed = new Subject<CheckPoint>();
    public IObservable<CheckPoint> Passed => _Passed;

    bool passedYet = false;
    public void Init(int index) => Index = index;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(TagNames.Hero) && !passedYet)
        {
            passedYet = true;
            _Passed.OnNext(this);
        }
    }
}

