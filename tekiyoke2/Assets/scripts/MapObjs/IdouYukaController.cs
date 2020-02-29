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
    float moveSpeed = 10;
    Vector3 moveVec = new Vector3();

    [SerializeField]
    int StopFrames = 60;
    int frames2StopNow = 0;

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

        moveVec = ( positionB - positionA ).normalized * moveSpeed;
    }

    void Update()
    {
        bool isTouchedByHero = col.IsTouching(filter2Hero);

        switch(state){

            case State.AtoB:
                if(MyMath.DistanceXY(yukaTF.position, positionB) < 10){
                    if(isTouchedByHero)
                        HeroDefiner.currentHero.additionalVelocities[this] = positionB - yukaTF.position + new Vector3(0,-100,0);
                    else
                        HeroDefiner.currentHero.additionalVelocities.Remove(this);

                    yukaRB.MovePosition(positionB);
                    frames2StopNow = StopFrames;
                    state = State.B;

                }else{
                    yukaRB.MovePosition(yukaTF.position + moveVec);

                    if(isTouchedByHero) HeroDefiner.currentHero.additionalVelocities[this] = moveVec + new Vector3(0,-1,0);
                    else                HeroDefiner.currentHero.additionalVelocities.Remove(this);
                }
                break;

            case State.B:   
                frames2StopNow --;
                if(frames2StopNow==0) state = State.BtoA;
                HeroDefiner.currentHero.additionalVelocities.Remove(this);
                break;

            case State.BtoA:
                if(MyMath.DistanceXY(yukaTF.position, positionA) < 10){
                    if(isTouchedByHero)
                        HeroDefiner.currentHero.additionalVelocities[this] = positionA - yukaTF.position + new Vector3(0,-1,0);
                    else
                        HeroDefiner.currentHero.additionalVelocities.Remove(this);

                    yukaRB.MovePosition(positionA);
                    frames2StopNow = StopFrames;
                    state = State.A;
                }else{
                    yukaRB.MovePosition(yukaTF.position - moveVec);
                    
                    if(isTouchedByHero) HeroDefiner.currentHero.additionalVelocities[this] = -moveVec + new Vector3(0,-1,0);
                    else                HeroDefiner.currentHero.additionalVelocities.Remove(this);
                }
                break;

            case State.A:
                frames2StopNow --;
                if(frames2StopNow==0) state = State.AtoB;
                HeroDefiner.currentHero.additionalVelocities.Remove(this);
                break;
        }
    }
}
