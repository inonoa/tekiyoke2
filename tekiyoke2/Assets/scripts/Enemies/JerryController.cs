using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;

///<summary>子(Sprite)だけが動いてしまい無限に上昇する</summary>
public class JerryController : EnemyController
{

    [SerializeField]
    float _Amplitude = 100;
    ///<summary>振幅</summary>
    public float Amplitude{
        get{ return _Amplitude; }
        set{ _Amplitude = value; }
    }

    [SerializeField]
    float _SpeedRate = 4;
    ///<summary>速度の倍率(はい)</summary>
    public float SpeedRate{
        get{ return _SpeedRate; }
        set{ _SpeedRate = value; }
    }

    ///<summary>中心の座標(Controllerの位置)</summary>
    Vector3 centerPosition;

    public bool isGoingUp = true;

    // Start is called before the first frame update
    new void Start(){
        base.Start();
        centerPosition = new Vector3(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(isGoingUp){
            if(transform.position.y > centerPosition.y+Amplitude-100){
                //上端
                float v = (float)Math.Sqrt(Math.Max(centerPosition.y+Amplitude - transform.position.y,0)) * SpeedRate/10;
                base.MovePos(0,v);
                if(transform.position.y >= centerPosition.y+Amplitude-1){
                    isGoingUp = false;
                }
            }else if(transform.position.y > centerPosition.y-Amplitude+100){
                //中間
                base.MovePos(0,SpeedRate);
            }else{
                //下端
                float v = (float)Math.Sqrt(Math.Max(transform.position.y - centerPosition.y+Amplitude,0)) * SpeedRate/10;
                base.MovePos(0,v);
            }
        }else{
            if(transform.position.y > centerPosition.y+Amplitude-100){
                //上端
                float v = (float)Math.Sqrt(Math.Max(centerPosition.y+Amplitude - transform.position.y,0)) * SpeedRate/10;
                base.MovePos(0,-v);
            }else if(transform.position.y > centerPosition.y-Amplitude+100){
                //中間
                base.MovePos(0,-SpeedRate);
            }else{
                //下端
                float v = (float)Math.Sqrt(Math.Max(transform.position.y - centerPosition.y+Amplitude,0)) * SpeedRate/10;
                base.MovePos(0,-v);
                if(transform.position.y <= centerPosition.y-Amplitude+1){
                    isGoingUp = true;
                }
            }
        }
    }
}
