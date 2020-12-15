using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class IdouYukaController : MonoBehaviour
{
    enum State{ AtoB, B, BtoA, A }
    State state = State.AtoB;

    [SerializeField] float moveSeconds = 2f;
    [SerializeField] float StopSeconds = 1f;
    

    [Space(10)]
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    Vector2 PositionA => A.position;
    Vector2 PositionB => B.position;
    
    [SerializeField] Transform yukaTF;
    Rigidbody2D yukaRB;
    Collider2D col;
    [SerializeField] ContactFilter2D filter2Hero = new ContactFilter2D();

    void Start()
    {
        yukaRB = yukaTF.GetComponent<Rigidbody2D>();
        col    = yukaTF.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        bool isTouchedByHero = col.IsTouching(filter2Hero);

        float dt = TimeManager.Current.FixedDeltaTimeExceptHero;
        
        var heroAdditionalVels = HeroDefiner.currentHero.additionalVelocities;
        Vector2 additionalVelOffset = new Vector2(0, -2);

        switch(state)
        {
        case State.AtoB:
            if(MyMath.DistanceXY(yukaTF.position, PositionB) <= NormalVelocity().magnitude * dt ||
               MyMath.ExceedB(yukaTF.position.ToVec2(), PositionA, PositionB))
            {
                yukaRB.MovePosition(PositionB);
                if(isTouchedByHero) heroAdditionalVels[this] = PositionB - yukaTF.position.ToVec2() + additionalVelOffset;
                else                heroAdditionalVels.Remove(this);
                
                Stop(atA: false);
            }
            else
            {
                yukaRB.MovePosition(yukaTF.position.ToVec2() + NormalVelocity() * dt);
                if(isTouchedByHero) heroAdditionalVels[this] = NormalVelocity() * dt + additionalVelOffset;
                else                heroAdditionalVels.Remove(this);
            }
            break;
        
        case State.BtoA:
            if(MyMath.DistanceXY(yukaTF.position, PositionA) <= NormalVelocity().magnitude * dt ||
               MyMath.ExceedB(yukaTF.position.ToVec2(), PositionB, PositionA))
            {
                yukaRB.MovePosition(PositionA);
                if(isTouchedByHero) heroAdditionalVels[this] = PositionA - yukaTF.position.ToVec2() + additionalVelOffset;
                else                heroAdditionalVels.Remove(this);
                
                Stop(atA: true);
            }
            else
            {
                yukaRB.MovePosition(yukaTF.position.ToVec2() - NormalVelocity() * dt);
                if(isTouchedByHero) heroAdditionalVels[this] = - NormalVelocity() * dt + additionalVelOffset;
                else                heroAdditionalVels.Remove(this);
            }
            break;
        }
    }
    
    Vector2 NormalVelocity() => ( PositionB - PositionA ) / moveSeconds;

    void Stop(bool atA)
    {
        state = atA ? State.A : State.B;
        Observable.TimerFrame(1, FrameCountType.FixedUpdate)
            .Subscribe(_ => HeroDefiner.currentHero.additionalVelocities.Remove(this))
            .AddTo(this);
        
        DOVirtual.DelayedCall
        (
           StopSeconds, 
           () => state = atA ? State.AtoB : State.BtoA,
           ignoreTimeScale: false
        )
        .GetPausable();
    }
}
