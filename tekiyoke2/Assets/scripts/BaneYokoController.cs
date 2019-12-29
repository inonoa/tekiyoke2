using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaneYokoController : MonoBehaviour
{
    [SerializeField]
    int fromTrigger2Push = 50;
    int frames2Push = 50;
    [SerializeField]
    float pushForce = 60;

    [SerializeField]
    ContactFilter2D filter = new ContactFilter2D();
    Collider2D col;

    void Start(){
        col = GetComponent<BoxCollider2D>();
    }

    void Update(){
        print(col.IsTouching(filter));
        if(col.IsTouching(filter)){
            frames2Push --;
            if(frames2Push==0){
                HeroDefiner.currentHero.States.Push(new StateJump(HeroDefiner.currentHero, jumpForce: pushForce)); //とりま
            }
        }else{
            frames2Push = fromTrigger2Push;
        }
    }
}
