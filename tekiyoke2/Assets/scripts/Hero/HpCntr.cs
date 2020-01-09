using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HpCntr : MonoBehaviour
{
    public Sprite img3;   public Sprite img2;   public Sprite img1;   public Sprite img0;
    public Sprite img3_2; public Sprite img2_1; public Sprite img1_0;
    public Image spr;

    private static int max_hp = 3;
    private bool isDamaging = false;
    private int framesAfterDamage = 0;

    ///<summary>ここを直接書き換えない</summary>
    public int hp = max_hp;

    public event EventHandler die;
    public event EventHandler damaged;
    private float[,] damagemove = new float[10,2]{{-20,6},
                                                       {-10,3},{0,0},{20,-10},{9,-5},
                                                       {0,0},{15,8},
                                                       {16,4},{9,-5},{3,-1}};

    new public GameObject camera;


    ///<summary>HPの増減はすべてここから。</summary>
    public int HP{
        get{return hp;}
        set{
            if(value<=0 && HP<=0) return;
            if(HP>value)damaged?.Invoke(this,EventArgs.Empty);
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
        damagemove = IikanjinoKansu(3);
        string hoge = "";
        Debug.Log(damagemove.Length);
        for(int i=0;i<15;i++){
            for(int j=0;j<2;j++){
                hoge += damagemove[i,j] + ",";
            }
            hoge += "\n";
        }
        Debug.Log(hoge);
    }

    float[,] IikanjinoKansu(float radius){
        Vector2[] vs = new Vector2[15];
        for(int i=0; i<5; i++){
            vs[3*i + 2] = new Vector2((float)Math.Cos(2*i) * (5-i)*(5-i) * radius, (float)Math.Sin(2*i) * (5-i)*(5-i) * radius);
            vs[3*i    ] = i==0 ? vs[2] * 1 / 5 : ( vs[3*i-1] * 4 + vs[3*i+2] ) / 5;
            vs[3*i + 1] = i==0 ? vs[2] * 4 / 5 : ( vs[3*i+2] * 4 + vs[3*i-1] ) / 5;
        }
        float[,] re = new float[30,2];
        re[0,0] = vs[0].x;
        re[0,1] = vs[0].y;
        re[1,0] = 0; re[1,1] = 0;
        for(int i=1; i<15; i++){
            re[2*i+1,0] = 0; re[2*i+1,1] = 0;
            re[2*i,0] = vs[i].x - vs[i-1].x;
            re[2*i,1] = vs[i].y - vs[i-1].y;
        }
        return re;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isDamaging){
            if(framesAfterDamage<30){
                camera.transform.localPosition += new Vector3(damagemove[framesAfterDamage,0],damagemove[framesAfterDamage,1]);
            }else if(framesAfterDamage==30){
                camera.transform.localPosition = HeroDefiner.CurrentHeroPastPos[0] + new Vector3(0,50,-200); 
                //短時間に複数回被弾したときに画面揺れの途中で揺れの状態が初めに戻って二重にずれてる、応急処置
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
