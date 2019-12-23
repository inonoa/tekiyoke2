using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameTimeCounter clock;
    public Animator doubleAnim;
    public GoalCurtainMover curtain;

    bool goaled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
            HeroDefiner.currentHero.CanMove = false;
            clock.DoesTick = false;
            doubleAnim.gameObject.SetActive(true);
            //curtain.gameObject.SetActive(true);

            SceneTransition.Start2ChangeState("StageChoiceScene", SceneTransition.TransitionType.Normal);

            doubleAnim.SetTrigger("runr");
            Debug.Log("GOAL!!!!");
        }
    }
}
