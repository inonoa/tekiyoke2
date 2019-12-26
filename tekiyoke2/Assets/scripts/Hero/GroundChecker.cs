using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>これの参照を直接持っていいのはHeroMoverだけです</summary>
public class GroundChecker : MonoBehaviour
{
    public bool IsOnGround = true;


    [SerializeField]
    ContactFilter2D filter;
    [SerializeField]
    new PolygonCollider2D collider;
    void Start() => collider = GetComponent<PolygonCollider2D>();
    void Update() => IsOnGround = collider.IsTouching(filter);
}
