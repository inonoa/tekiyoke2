using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HpCntr : MonoBehaviour
{
    public Sprite img3;
    public Sprite img3_2;
    public Sprite img2;
    public Sprite img2_1;
    public Sprite img1;
    public Sprite img0;
    public Sprite img1_0;
    public Image spr;
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
                hp=0;
                spr.sprite = img0;
                }
            else{
                hp = Math.Min(3,value);
                if(value==1){
                    spr.sprite = img1;
                }else if(value==2){
                    spr.sprite = img2;
                }else{
                    spr.sprite = img3;
                }
            }
        }
    }

    ///<summary>全回復</summary>
    public void FullRecover(){
        HP = max_hp;
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
