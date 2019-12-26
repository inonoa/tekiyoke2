﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckerR : MonoBehaviour
{

    public bool CanKick { get; private set; } = false;

    [SerializeField]
    private ContactFilter2D filter;
    private BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CanKick = col.IsTouching(filter);
    }
}
