using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainMover : MonoBehaviour
{

    public enum CState{
        GameStart, ToBeInActive, Dying
    }

    public CState state = CState.GameStart;

    public event EventHandler heroRespawn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch(state){
            case CState.GameStart:
                gameObject.transform.position += new Vector3(50,0);
                if(gameObject.transform.localPosition.x>4000){
                    state = CState.ToBeInActive;
                }
                break;
            case CState.ToBeInActive:
                gameObject.SetActive(false);
                break;
            case CState.Dying:
                gameObject.transform.position += new Vector3(50,0);
                if(gameObject.transform.localPosition.x>4000){
                    state = CState.ToBeInActive;
                }else if(gameObject.transform.localPosition.x>400 && gameObject.transform.localPosition.x<500){
                    heroRespawn?.Invoke(this,EventArgs.Empty);
                }
                break;
        }
    }
}
