using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpCntr : MonoBehaviour
{
    Slider slider;

    private int hp = 100;
    public int HP{
        get{return hp;}
        set{
            if(value<=0){
                hp = 0;
            }
            else if(value >= 100){
                hp = 100;
            }
            else{
                hp = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = HP;
    }
}
