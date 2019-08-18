using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    protected void MovePos(Rigidbody2D rbody,float v_x, float v_y){
        rbody.MovePosition(new Vector2(rbody.transform.position.x + v_x*Time.timeScale,
                                       rbody.transform.position.y + v_y*Time.timeScale));
    }

    void Start(){
        
    }

    void Update(){
        //
    }
}
