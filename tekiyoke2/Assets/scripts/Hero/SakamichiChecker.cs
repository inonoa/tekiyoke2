using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakamichiChecker : MonoBehaviour
{
    public bool OnSakamichi{ get => OnSakamichiL || OnSakamichiR; }
    bool _OnSakamichiR = false;
    public bool OnSakamichiR{ get => _OnSakamichiR; }
    bool _OnSakamichiL = false;
    public bool OnSakamichiL{ get => _OnSakamichiL; }

    [SerializeField]
    ContactFilter2D filterR;
    [SerializeField]
    ContactFilter2D filterL;
    [SerializeField]
    new PolygonCollider2D collider;
    void Update(){
        _OnSakamichiL = collider.IsTouching(filterL);
        _OnSakamichiR = collider.IsTouching(filterR);
    }
}
