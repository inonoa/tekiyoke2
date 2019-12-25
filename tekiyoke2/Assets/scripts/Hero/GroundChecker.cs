using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isOnGround = true;
    public PolygonCollider2D pol;
    public HeroMover heroMover;
    // Start is called before the first frame update
    void Start()
    {
        pol = GetComponent<PolygonCollider2D>();
        heroMover = transform.parent.GetComponent<HeroMover>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other){

        if(other.tag=="Terrain") {
            isOnGround = true;
        }
        
        if(other.tag=="Ultrathin"){
            if(heroMover.velocity.y<=0){
                isOnGround = true;
            }
        }
        
    }


    void OnTriggerEnter2D(Collider2D other){
        //すり抜け床の場合だけEnterでも判定してる(Stayは上)がなんで(要検証)
        if(other.tag=="Ultrathin"){
            if(heroMover.velocity.y<=0){
                isOnGround = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag=="Terrain" || (other.tag=="Ultrathin" && heroMover.velocity.y>=0)) isOnGround = false;
    }
}
