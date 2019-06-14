using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
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
            heroMover.JumpCount = 1; heroMover.isOnGround = true; heroMover.speedY = 0;
            heroMover.IsFromWall = false; heroMover.isBendingBack = false; heroMover.SpeedX = 0; heroMover.speedY = 0;
        }
        
        if(other.tag=="Ultrathin"){
            if(heroMover.speedY<=0){
                if(heroMover.speedY!=0) Debug.Log(heroMover.speedY.ToString()+"でぶつかったよ");
                heroMover.JumpCount = 1; heroMover.isOnGround = true; heroMover.speedY = 0;
                heroMover.IsFromWall = false; heroMover.isBendingBack = false; heroMover.SpeedX = 0; heroMover.speedY = 0;
            }
        }
        
    }


    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Ultrathin"){
            if(heroMover.speedY!=0) Debug.Log(heroMover.speedY.ToString()+"でぶつかったよ");
            if(heroMover.speedY<=0){
                heroMover.JumpCount = 1; heroMover.isOnGround = true; heroMover.speedY = 0;
                heroMover.IsFromWall = false; heroMover.isBendingBack = false; heroMover.SpeedX = 0; heroMover.speedY = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag=="Terrain" || (other.tag=="Ultrathin" && heroMover.speedY>=0)) heroMover.isOnGround = false;
    }
}
