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
        //(すり抜け床以外)着地したらジャンプ周りの値をリセット
        if(other.tag=="Terrain") {
            heroMover.JumpCount = 1; heroMover.isOnGround = true; heroMover.speedY = 0;
            heroMover.IsFromWall = false; heroMover.isBendingBack = false; heroMover.SpeedX = 0; heroMover.speedY = 0;
        }
        
        //すり抜け床の場合は下向きに落ちてるかどうかを確認
        if(other.tag=="Ultrathin"){
            if(heroMover.speedY<=0){
                heroMover.JumpCount = 1; heroMover.isOnGround = true; heroMover.speedY = 0;
                heroMover.IsFromWall = false; heroMover.isBendingBack = false; heroMover.SpeedX = 0; heroMover.speedY = 0;
            }
        }
        
    }


    void OnTriggerEnter2D(Collider2D other){
        //すり抜け床の場合だけEnterでも判定してる(Stayは上)がなんで(要検証)
        if(other.tag=="Ultrathin"){
            if(heroMover.speedY<=0){
                heroMover.JumpCount = 1; heroMover.isOnGround = true; heroMover.speedY = 0;
                heroMover.IsFromWall = false; heroMover.isBendingBack = false; heroMover.SpeedX = 0; heroMover.speedY = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        //接地解除
        if(other.tag=="Terrain" || (other.tag=="Ultrathin" && heroMover.speedY>=0)) heroMover.isOnGround = false;
    }
}
