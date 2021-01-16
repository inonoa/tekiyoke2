using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class FixedArtillery : SerializedMonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [SerializeField] Collider2D heroSensor;
    [SerializeField] FixedArtilleryBeam beam;
    [field: SerializeField, LabelText(nameof(DPCD))] public DPinEnemy DPCD { get; private set; }

    [SerializeField] float fromReadyToAttack = 1f;
    [SerializeField] float fromSlowToIdle = 1f;
    
    public enum EState
    {
        Idling, Ready, Attacking, SlowingDown
    }
    
    [SerializeField, ReadOnly] ReactiveProperty<EState> _state = new ReactiveProperty<EState>(EState.Idling);
    public IReadOnlyReactiveProperty<EState> State => _state;
    
    [SerializeField, ReadOnly] BoolReactiveProperty heroIn = new BoolReactiveProperty(false);

    void Start()
    {
        State.Pairwise().Subscribe(state =>
        {
            switch (state.Current)
            {
            case EState.Idling:
                break;
            case EState.Ready:
                beam.BeReady().Subscribe(_ => Attack()).AddTo(this);
                break;
            case EState.Attacking:
                break;
            case EState.SlowingDown:
                var complete = DOVirtual.DelayedCall(fromSlowToIdle, () => _state.Value = EState.Idling);
                State.Skip(2).Take(1).Subscribe(next => complete.Kill());
                break;
            }
        });

        heroSensor.OnTriggerEnter2DAsObservable().Where(col => col.CompareTag(TagNames.Hero)).Subscribe(_ => heroIn.Value = true);
        heroSensor.OnTriggerExit2DAsObservable().Where(col => col.CompareTag(TagNames.Hero)).Subscribe(_ => heroIn.Value = false);
        
        heroIn
            .Where(val => val)
            .Where(_ => State.Value == EState.Idling || State.Value == EState.SlowingDown)
            .Subscribe(_ => _state.Value = EState.Ready)
            .AddTo(this);
    }

    void Attack()
    {
        beam.StartShoot()
            .Subscribe(_ =>
            {
                _state.Value = heroIn.Value ? EState.Ready : EState.SlowingDown;
            })
            .AddTo(this);
        _state.Value = EState.Attacking;
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