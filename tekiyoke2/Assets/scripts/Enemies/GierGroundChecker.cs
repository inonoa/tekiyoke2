using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>接地判定のためにあるクラスだが、Update()とOnTriggerEnter2D()が交互に呼ばれている前提になってて不安</summary>
public class GierGroundChecker : MonoBehaviour
{
    public bool IsOnGround { get; set; } = false;
    bool onGroundInThisFrame = false;

    // Update is called once per frame
    void Update()
    {
        if(onGroundInThisFrame) IsOnGround = true;
        else IsOnGround = false;

        onGroundInThisFrame = false;
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="Terrain" || other.tag=="Ultrathin"){
            onGroundInThisFrame = true;
        }
    }
}
