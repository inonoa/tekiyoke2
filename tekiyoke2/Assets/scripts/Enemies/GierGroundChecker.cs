using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>接地判定のためにあるクラス。分ける必要ある？？</summary>
public class GierGroundChecker : MonoBehaviour
{
    public bool IsOnGround{ get; set; } = false;
    
    [SerializeField]
    ContactFilter2D filter;
    Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsOnGround = col.IsTouching(filter);
    }
}
