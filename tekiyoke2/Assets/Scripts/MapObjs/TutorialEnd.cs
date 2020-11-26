using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    bool goaled = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(TagNames.Hero) && !goaled)
        {
            goaled = true;
            HeroDefiner.currentHero.OnGoal();
            CameraController.CurrentCamera.Freeze(10);
            GameTimeCounter.CurrentInstance.DoesTick = false;
            MemoryOverDeath.Instance.Clear();

            SceneTransition.Start2ChangeScene("Draft1", SceneTransition.TransitionType.WhiteOut);
        }
    }
}
