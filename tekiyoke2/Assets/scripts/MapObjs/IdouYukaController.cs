using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdouYukaController : MonoBehaviour
{
    Vector3 positionA;
    Vector3 positionB;
    Rigidbody2D yukaRB;
    Transform yukaTF;

    enum State{ AtoB, B, BtoA, A }
    State state = State.AtoB;

    [SerializeField]
    float moveSpeedPerSec = 400;
    Vector3 moveVec = new Vector3();

    [SerializeField]
    float StopSeconds = 1;
    float seconds2StopNow = 0;

    [SerializeField]
    ContactFilter2D filter2Hero = new ContactFilter2D();
    Collider2D col;

    void Start()
    {
        positionA = transform.Find("PositionA").position;
        positionB = transform.Find("PositionB").position;
        yukaTF    = transform.Find("Yuka");
        yukaRB    = yukaTF.GetComponent<Rigidbody2D>();
        col       = yukaTF.GetComponent<Collider2D>();

        moveVec = ( positionB - positionA ).normalized * moveSpeedPerSec;
    }

    void FixedUpdate()
    {
        bool isTouchedByHero = col.IsTouching(filter2Hero);

        float dt = TimeManager.Current.FixedDeltaTimeExceptHero;

        switch(state){

            case State.AtoB:
                if(MyMath.DistanceXY(yukaTF.position, positionB) <= moveSpeedPerSec / 50 ||
                    MyMath.ExceedB(yukaTF.position.ToVec2(), positionA.ToVec2(), positionB.ToVec2()))
                {
                    if(isTouchedByHero)
                        HeroDefiner.currentHero.additionalVelocities[this] = positionB - yukaTF.position + new Vector3(0,-100,0);
                    else
                        HeroDefiner.currentHero.additionalVelocities.Remove(this);

                    yukaRB.MovePosition(positionB);
                    seconds2StopNow = StopSeconds;
                    state = State.B;

                }
                else
                {
                    yukaRB.MovePosition(yukaTF.position + moveVec * dt);

                    if(isTouchedByHero) HeroDefiner.currentHero.additionalVelocities[this] = moveVec * dt + new Vector3(0,-1,0);
                    else                HeroDefiner.currentHero.additionalVelocities.Remove(this);
                }
                break;

            case State.B:   
                seconds2StopNow -= TimeManager.Current.DeltaTimeExceptHero;
                if(seconds2StopNow <= 0) state = State.BtoA;
                HeroDefiner.currentHero.additionalVelocities.Remove(this);
                break;

            case State.BtoA:
                if(MyMath.DistanceXY(yukaTF.position, positionA) <= moveSpeedPerSec / 50 ||
                    MyMath.ExceedB(yukaTF.position.ToVec2(), positionB.ToVec2(), positionA.ToVec2()))
                {
                    if(isTouchedByHero)
                        HeroDefiner.currentHero.additionalVelocities[this] = positionA - yukaTF.position + new Vector3(0,-1,0);
                    else
                        HeroDefiner.currentHero.additionalVelocities.Remove(this);

                    yukaRB.MovePosition(positionA);
                    seconds2StopNow = StopSeconds;
                    state = State.A;
                }
                else
                {
                    yukaRB.MovePosition(yukaTF.position - moveVec * dt);
                    
                    if(isTouchedByHero) HeroDefiner.currentHero.additionalVelocities[this] = - moveVec * dt + new Vector3(0,-1,0);
                    else                HeroDefiner.currentHero.additionalVelocities.Remove(this);
                }
                break;

            case State.A:
                seconds2StopNow -= TimeManager.Current.DeltaTimeExceptHero;
                if(seconds2StopNow <= 0) state = State.AtoB;
                HeroDefiner.currentHero.additionalVelocities.Remove(this);
                break;
        }
    }
}
