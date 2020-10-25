using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaneYokoController : MonoBehaviour
{
    [SerializeField]
    bool push2Right = true;

    [SerializeField]
    int fromTrigger2Push = 50;
    int frames2Push = 50;

    [SerializeField]
    int frames2BeStoppable = 20;

    [SerializeField]
    float pushForce = 60;

    [SerializeField]
    ContactFilter2D filter = new ContactFilter2D();
    Collider2D col;

    [SerializeField] SpriteRenderer shaderSpriteRenderer;
    Material mat;

    void Start(){
        col = GetComponent<BoxCollider2D>();
        mat = shaderSpriteRenderer.material;
    }

    void Update(){
        if(col.IsTouching(filter)){
            mat.SetInt("_HeroOn", 1);
            frames2Push --;
            if(frames2Push==0){
                HeroDefiner.currentHero.PushedByBaneYoko(push2Right, pushForce);
                mat.SetInt("_Flash", 1);
                DOVirtual.DelayedCall(0.2f, () => mat.SetInt("_Flash", 0));
            }
        }else{
            mat.SetInt("_HeroOn", 0);
            frames2Push = fromTrigger2Push;
        }
    }
}
