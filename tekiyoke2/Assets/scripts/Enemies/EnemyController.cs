using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyController : MonoBehaviour
{
    protected Rigidbody2D rBody;

    protected void MovePos(float v_x, float v_y){
        rBody.MovePosition(new Vector2(rBody.transform.position.x + v_x*Time.timeScale,
                                       rBody.transform.position.y + v_y*Time.timeScale));
    }

    protected void Start(){
        rBody = GetComponent<Rigidbody2D>();
    }
}
