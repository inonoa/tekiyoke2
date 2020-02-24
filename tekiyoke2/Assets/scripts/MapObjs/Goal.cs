using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameTimeCounter clock;
    public Animator doubleAnim;
    public Curtain4SceneEndMover curtain;

    bool goaled = false;

    // Update is called once per frame
    void Update()
    {
        if(goaled){
            doubleAnim.transform.position += new Vector3(8,0,0);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Player" && !goaled){
            goaled = true;
            HeroDefiner.currentHero.spriteRenderer.enabled = false;
            HeroDefiner.currentHero.IsFrozen = true;
            clock.DoesTick = false;
            doubleAnim.gameObject.SetActive(true);

            SceneTransition.Start2ChangeState("ResultScene", SceneTransition.TransitionType.WindAndBlur);

            doubleAnim.SetTrigger("runr");
        }
    }
}
