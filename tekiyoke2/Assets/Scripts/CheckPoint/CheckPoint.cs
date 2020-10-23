using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class CheckPoint : MonoBehaviour
{
    [field: SerializeField] [field: RenameField("Name")]
    public string Name{ get; private set; }

    Subject<CheckPoint> _Passed = new Subject<CheckPoint>();
    public IObservable<CheckPoint> Passed => _Passed;

    bool passedYet = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(TagNames.Hero) && !passedYet)
        {
            passedYet = true;
            _Passed.OnNext(this);
        }
    }
}
