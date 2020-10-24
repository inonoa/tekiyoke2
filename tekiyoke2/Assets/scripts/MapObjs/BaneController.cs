using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaneController : MonoBehaviour
{
    [SerializeField] int fromTrigger2Fly = 50;
    int frames2Fly = 50;
    [SerializeField] float jumpForce = 60;

    [SerializeField] ContactFilter2D filter = new ContactFilter2D();
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
            frames2Fly --;
            if(frames2Fly==0){
                HeroDefiner.currentHero.ForceJump(jumpForce);
                mat.SetInt("_Flash", 1);
                DOVirtual.DelayedCall(0.2f, () => mat.SetInt("_Flash", 0)).FollowTimeScale(aroundHero: false);
            }
        }else{
            mat.SetInt("_HeroOn", 0);
            frames2Fly = fromTrigger2Fly;
        }
    }
}
