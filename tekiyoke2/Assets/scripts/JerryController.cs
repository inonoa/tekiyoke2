using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryController : MonoBehaviour
{
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
    int _Period = 360;
    ///<summary>周期</summary>
    public int Period{
        get{
            return _Period;
        }
        set{
            _Period = value;
        }
    }
    [SerializeField]
    int _Phase = 360;
    ///<summary>位相</summary>
    public int Phase{
        get{
            return _Phase;
        }
        set{
            int toset = value;
            //周期をこえるとき0に戻る
            while(toset<Period){
                toset -= Period;
            }
            _Phase = toset;
        }
    }
    ///<summary>最初の座標を元に移動する</summary>
    Vector3 defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
