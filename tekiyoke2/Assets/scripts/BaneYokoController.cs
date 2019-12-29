using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaneYokoController : MonoBehaviour
{
    [SerializeField]
    bool push2Right = true;

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
        if(col.IsTouching(filter)){
            frames2Push --;
            if(frames2Push==0){
                HeroDefiner.currentHero.States.Push(new StateBaneYoko(HeroDefiner.currentHero, push2Right, pushForce)); //とりま
            }
        }else{
            frames2Push = fromTrigger2Push;
        }
    }
}
