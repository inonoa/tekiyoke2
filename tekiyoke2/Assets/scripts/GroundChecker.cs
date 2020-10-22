using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool IsOnGround{ get; private set; } = true;


    [SerializeField]
    ContactFilter2D filter = new ContactFilter2D();
    new PolygonCollider2D collider;
    void Start() => collider = GetComponent<PolygonCollider2D>();
    void Update()
    {
        IsOnGround = collider.IsTouching(filter);
    }
}
