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
    private bool isDamaging = false;
    private int framesAfterDamage = 0;

    ///<summary>ここを直接書き換えない</summary>
    public int hp = max_hp;

    public event EventHandler die;
    private readonly int[,] damagemove = new int[10,2]{{-10,3},{0,0},{0,0},{0,0},{13,-5},{0,0},{0,0},{-7,4},{0,0},{4,-2}};

    new public GameObject camera;


    ///<summary>HPの増減はすべてここから。</summary>
    public int HP{
        get{return hp;}
        set{
            if(value<=0){
                die?.Invoke(this,EventArgs.Empty);
                hp=0;
                spr.sprite = img1_0;
                framesAfterDamage = 0;
                isDamaging = true;
                spr.color = new Color(1,1,1,1);
                }
            else{
                hp = Math.Min(3,value);
                if(value==1){
                    spr.sprite = img2_1;
                    isDamaging = true;
                    framesAfterDamage = 0;
                    spr.color = new Color(1,1,1,1);
                }else if(value==2){
                    spr.sprite = img3_2;
                    isDamaging = true;
                    framesAfterDamage = 0;
                    spr.color = new Color(1,1,1,1);
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
        if(this.isDamaging){
            if(framesAfterDamage<10){
                camera.transform.localPosition += new Vector3(damagemove[framesAfterDamage,0],damagemove[framesAfterDamage,1]);
            }else if(framesAfterDamage==10){
                camera.transform.localPosition = new Vector3(0,50,-100);
            }
            framesAfterDamage ++;
            if(framesAfterDamage==20 || framesAfterDamage==21){
                if(HP==0)spr.sprite = img0;
                else if(HP==1)spr.sprite = img1;
                else if(HP==2)spr.sprite = img2;
                else{Debug.Log("HPが0,1,2じゃないのにダメージを受けたことになってるよ！");}
            }else if(framesAfterDamage==60){
                spr.color = new Color(1,1,1,180f/255f);
                isDamaging = false;
                framesAfterDamage = 0;
            }
        }
    }
}
