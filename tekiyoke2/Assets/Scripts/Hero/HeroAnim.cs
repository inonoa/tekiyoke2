using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HeroAnim : MonoBehaviour
{
    Animator animator;
    [SerializeField] HeroMover hero;

    const string NO_TRIGGER = "No Trigger";
    string trigger = NO_TRIGGER;

    public void SetTrigger(string id)
    {
        SetTriggerManually(id + (hero.WantsToGoRight ? "r" : "l"));
    }

    public void SetTriggerManually(string id)
    {
        trigger = id;
    }

    void LateUpdate()
    {
        ApplyTrigger();
    }

    void ApplyTrigger()
    {
        if(trigger != NO_TRIGGER)
        {
            animator.SetTrigger(trigger);
            trigger = NO_TRIGGER;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        hero.TimeManager.HeroTimeScaleRelative
            .Subscribe(scale =>
            {
                animator.speed = scale;
            });
    }
}
