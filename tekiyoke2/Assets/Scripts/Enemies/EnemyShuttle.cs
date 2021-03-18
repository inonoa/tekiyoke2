using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class EnemyShuttle : MonoBehaviour, ISpawnsNearHero, IHaveDPinEnemy
{
    [SerializeField] bool goToRight;
    [SerializeField] float topSpeed = 200;
    [SerializeField] float durationToTopSpeed = 0.5f;
    
    [Space(10)]
    [SerializeField] Collider2D heroSensor;
    [SerializeField] ShuttleView view;
    [SerializeField] new Rigidbody2D rigidbody;

    [field: SerializeField]
    public DPinEnemy DPCD { get; private set; }

    enum State
    {
        Inactive, Active
    }
    [SerializeField, ReadOnly] State state = State.Inactive;

    void Start()
    {
        heroSensor.OnTriggerEnter2DAsObservable()
            .Where(other => other.CompareTag(Tags.HeroCenter))
            .Take(1)
            .Subscribe(_ =>
            {
                float launchDelay = view.OnHeroDetected();
                DOVirtual.DelayedCall
                (
                    launchDelay,
                    () => state = State.Active,
                    false
                )
                .GetPausable()
                .AddTo(this);
            })
            .AddTo(this);
    }

    void Update()
    {
        switch (state)
        {
            case State.Active:
                float acceleration = topSpeed / durationToTopSpeed * TimeManager.Current.DeltaTimeExceptHero;
                var nextVel = rigidbody.velocity.x + (goToRight ? acceleration : -acceleration);
                rigidbody.velocity = new Vector2(Mathf.Clamp(nextVel, -topSpeed, topSpeed), 0);
                break;
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
