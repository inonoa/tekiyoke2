using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GroundChecker : MonoBehaviour
{
    bool _IsOnGround = false;
    public bool IsOnGround
    {
        get => _IsOnGround;
        private set
        {
            if(!_IsOnGround && value) _OnLand.OnNext(Unit.Default);
            _IsOnGround = value;
        }
    }

    Subject<Unit> _OnLand = new Subject<Unit>();
    public IObservable<Unit> OnLand => _OnLand;


    [SerializeField]
    ContactFilter2D filter = new ContactFilter2D();
    new PolygonCollider2D collider;
    void Start() => collider = GetComponent<PolygonCollider2D>();
    void Update()
    {
        IsOnGround = collider.IsTouching(filter);
    }
}
