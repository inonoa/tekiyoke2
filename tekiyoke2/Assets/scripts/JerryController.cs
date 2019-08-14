using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryController : MonoBehaviour
{
    [SerializeField]
    float _Amplitude = 100;
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
    public int Period{
        get{
            return _Period;
        }
        set{
            _Period = value;
        }
    }
    Vector3 defaultPosition;

    int phase = 0;

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
