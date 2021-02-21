using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] DraftManager draftManager;
    [SerializeField] ScoreHolder scoreHolder;

    bool goaled = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Tags.Hero) && !goaled)
        {
            goaled = true;
            HeroDefiner.currentHero.OnGoal();
            CameraController.Current.Freeze(10);
            GameTimeCounter.CurrentInstance.DoesTick = false;
            MemoryOverDeath.Instance.Clear();
            scoreHolder.Set(new StagePlayData(draftManager.StageIndex, GameTimeCounter.CurrentInstance.Seconds));

            SceneTransition.Start2ChangeScene("ResultScene", SceneTransition.TransitionType.WindAndBlur);
        }
    }
}
