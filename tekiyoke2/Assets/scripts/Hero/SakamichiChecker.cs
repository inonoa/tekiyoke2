using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>これの参照を直接持っていいのはHeroMoverだけです</summary>
public class SakamichiChecker : MonoBehaviour
{
    public bool OnSakamichi{ get => OnSakamichiL || OnSakamichiR; }
    public bool OnSakamichiR{ get => _OnSakamichiR; }
    public bool OnSakamichiL{ get => _OnSakamichiL; }


    bool _OnSakamichiR = false;
    bool _OnSakamichiL = false;

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
