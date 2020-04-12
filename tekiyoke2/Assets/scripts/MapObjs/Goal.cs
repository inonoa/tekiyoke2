using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    int stageIdx;
    public Animator doubleAnim;
    public Curtain4SceneEndMover curtain;

    bool goaled = false;

    void Start(){

        switch(this.gameObject.scene.name){
            case "Draft1": stageIdx = 0; break;
            case "Draft2": stageIdx = 1; break;
            case "Draft3": stageIdx = 2; break;
            default: stageIdx = 0; break;
        }
    }

    void Update()
    {
        if(goaled){
            doubleAnim.transform.position += new Vector3(8,0,0);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Player" && !goaled){
            goaled = true;
            HeroDefiner.currentHero.States.Push(new StateRun(HeroDefiner.currentHero));
            HeroDefiner.currentHero.CanMove = false;
            CameraController.CurrentCamera.Freeze(50 * 10);
            GameTimeCounter.CurrentInstance.DoesTick = false;
            ScoreHolder.Instance.clearTimesLast[stageIdx] = GameTimeCounter.CurrentInstance.Seconds;
            MemoryOverDeath.Instance.Clear();
            //doubleAnim.gameObject.SetActive(true);

            SceneTransition.Start2ChangeState("ResultScene", SceneTransition.TransitionType.WindAndBlur);

            doubleAnim.SetTrigger("runr");
        }
    }
}
