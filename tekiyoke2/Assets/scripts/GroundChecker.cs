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
        if(other.tag!="Player") heroMover.JumpCount = 1; heroMover.isOnGround = true; heroMover.speedY = 0;
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag!="Player") heroMover.isOnGround = false;
    }
}
