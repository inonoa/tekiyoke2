using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckerR : MonoBehaviour
{

    public bool canJump = false;

    private BoxCollider2D col;
    private HeroMover hero;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        hero = transform.parent.GetComponent<HeroMover>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other){
        canJump = true;
    }
    void OnTriggerExit2D(Collider2D other){
        canJump = false;
    }
}
