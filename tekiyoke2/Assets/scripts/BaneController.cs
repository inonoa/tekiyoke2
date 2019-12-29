using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaneController : MonoBehaviour
{
    [SerializeField]
    int fromTrigger2Fly = 50;
    int frames2Fly = 50;
    [SerializeField]
    float jumpForce = 60;

    [SerializeField]
    ContactFilter2D filter = new ContactFilter2D();
    Collider2D col;

    void Start(){
        col = GetComponent<BoxCollider2D>();
    }

    void Update(){
        print(col.IsTouching(filter));
        if(col.IsTouching(filter)){
            frames2Fly --;
            if(frames2Fly==0){
                HeroDefiner.currentHero.States.Push(new StateJump(HeroDefiner.currentHero, jumpForce: jumpForce)); //とりま
            }
        }else{
            frames2Fly = fromTrigger2Fly;
        }
    }
}
