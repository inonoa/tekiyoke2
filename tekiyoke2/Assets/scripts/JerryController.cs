using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryController : MonoBehaviour
{
    Transform jerryTf;
    Rigidbody2D jerryRb;

    [SerializeField]
    float _Amplitude = 100;
    ///<summary>振幅</summary>
    public float Amplitude{
        get{
            return _Amplitude;
        }
        set{
            _Amplitude = value;
        }
    }

    [SerializeField]
    float _SpeedRate = 2;
    ///<summary>速度の倍率(はい)</summary>
    public float SpeedRate{
        get{
            return _SpeedRate;
        }
        set{
            _SpeedRate = value;
        }
    }

    ///<summary>中心の座標(Controllerの位置)</summary>
    Vector3 centerPosition;

    public bool isGoingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        centerPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        jerryTf = GameObject.Find("Jerry").transform;
        jerryRb = jerryTf.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGoingUp){
            if(jerryTf.position.y > centerPosition.y+Amplitude-100){
                //上端
                float v = (centerPosition.y+Amplitude - jerryTf.position.y) * SpeedRate/100;
                jerryRb.MovePosition(new Vector2(jerryTf.position.x,jerryTf.position.y + v));
                if(jerryTf.position.y >= centerPosition.y+Amplitude){
                    isGoingUp = false;
                }
            }else if(jerryTf.position.y > centerPosition.y-Amplitude+100){
                //中間
                jerryRb.MovePosition(new Vector2(jerryTf.position.x,jerryTf.position.y + SpeedRate));
            }else{
                //下端
                float v = (jerryTf.position.y - centerPosition.y+Amplitude) * SpeedRate/100;
                jerryRb.MovePosition(new Vector2(jerryTf.position.x,jerryTf.position.y + v));
            }
        }else{
            if(jerryTf.position.y > centerPosition.y+Amplitude-100){
                //上端
                float v = (centerPosition.y+Amplitude - jerryTf.position.y) * SpeedRate/100;
                jerryRb.MovePosition(new Vector2(jerryTf.position.x,jerryTf.position.y - v));
            }else if(jerryTf.position.y > centerPosition.y-Amplitude+100){
                //中間
                jerryRb.MovePosition(new Vector2(jerryTf.position.x,jerryTf.position.y - SpeedRate));
            }else{
                //下端
                float v = (jerryTf.position.y - centerPosition.y+Amplitude) * SpeedRate/100;
                jerryRb.MovePosition(new Vector2(jerryTf.position.x,jerryTf.position.y - v));
                if(jerryTf.position.y <= centerPosition.y-Amplitude){
                    isGoingUp = true;
                }
            }
        }
    }
}
