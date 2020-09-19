using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckerR : MonoBehaviour
{

    public bool CanKick { get; private set; } = false;

    [SerializeField]
    private ContactFilter2D filter = new ContactFilter2D();
    private BoxCollider2D col;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        CanKick = col.IsTouching(filter);
    }
}
