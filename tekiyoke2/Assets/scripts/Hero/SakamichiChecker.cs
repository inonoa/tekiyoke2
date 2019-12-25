using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakamichiChecker : MonoBehaviour
{
    public bool OnSakamichi{ get => onSakamichiL || onSakamichiR; }
    bool onSakamichiR = false;
    bool onSakamichiL = false;

    [SerializeField]
    ContactFilter2D filterR;
    [SerializeField]
    ContactFilter2D filterL;
    [SerializeField]
    new PolygonCollider2D collider;
    void Update(){
        onSakamichiL = collider.IsTouching(filterL);
        onSakamichiR = collider.IsTouching(filterR);
    }
}
