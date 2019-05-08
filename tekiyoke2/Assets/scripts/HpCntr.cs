using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HpCntr : MonoBehaviour
{
    public Image hpImg1;
    public Image hpImg2;
    public Image hpImg3;
    private static int max_hp = 3;

    ///<summary>ここを直接書き換えない</summary>
    public int hp = max_hp;

    public event EventHandler die;


    ///<summary>HPの増減はすべてここから。</summary>
    public int HP{
        get{return hp;}
        set{
            if(value<=0){
                die?.Invoke(this,EventArgs.Empty);
                hp=max_hp;
                hpImg1.gameObject.SetActive(true); hpImg2.gameObject.SetActive(true); hpImg3.gameObject.SetActive(true);
                }
            else{
                hp = value;
                if(value==1){hpImg1.gameObject.SetActive(true); hpImg2.gameObject.SetActive(false); hpImg3.gameObject.SetActive(false);}
                else if(value==2){hpImg1.gameObject.SetActive(true); hpImg2.gameObject.SetActive(true); hpImg3.gameObject.SetActive(false);}
                else if(value==3){hpImg1.gameObject.SetActive(true); hpImg2.gameObject.SetActive(true); hpImg3.gameObject.SetActive(true);}
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
