using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HpCntr : MonoBehaviour
{
    [SerializeField] Sprite img3; [SerializeField] Sprite img2; [SerializeField] Sprite img1; [SerializeField] Sprite img0;
    [SerializeField] Sprite img3_2; [SerializeField] Sprite img2_1; [SerializeField] Sprite img1_0;
    [SerializeField] Sprite img2_3; [SerializeField] Sprite img1_2;
    [SerializeField] Image spr;

    private static int max_hp = 3; //正直3で決め打ってるし必要ある？
    private bool isDamaging = false;
    private int framesAfterDamage = 0;

    private int framesAfterRecover = 0;


    static readonly int mutekiFramesAfterDamage = 100;
    private bool _CanBeDamaged = true;
    public bool CanBeDamaged{
        get => _CanBeDamaged && (framesAfterDamage > mutekiFramesAfterDamage);
        set => _CanBeDamaged = value;
    }

    ///<summary>ここを直接書き換えない</summary>
    int hp = max_hp;

    public event EventHandler die;
    public event EventHandler damaged;
    public event EventHandler hpChanged;
    private float[,] damagemove = {{20,0},{0,0},{0,0},{-40,10},{0,0},{0,0},{10,-30},{0,0},{0,0},{15,30},{0,0},{0,0},{-5,-10}};

    new CameraController camera;

    ///<summary>HPの増減はすべてここから。</summary>
    public void ChangeHP(int value){
        if(value <= 0 && HP <= 0) return;
        if(value >= 3 && HP >= 3) return;

        if(HP > value){
            OnDamaged();
            damaged?.Invoke(this, EventArgs.Empty);

            if(value <= 0){
                die?.Invoke(this,EventArgs.Empty);
                hp=0;
                spr.sprite = img1_0;
            }
            else{
                hp = Math.Min(3,value);

                if(value==1)      spr.sprite = img2_1;
                else if(value==2) spr.sprite = img3_2;
            }
            hpChanged?.Invoke(this, EventArgs.Empty);
            
        }else if(HP < value){

            if(value >= 3){
                hp = 3;
                spr.sprite = img2_3;
                framesAfterRecover = 0;
                spr.color = new Color(1,1,1,1);
            }else if(value==2){
                hp = 2;
                spr.sprite = img1_2;
                framesAfterRecover = 0;
                spr.color = new Color(1,1,1,1);
            }
            hpChanged?.Invoke(this, EventArgs.Empty);
        }

        void OnDamaged(){
            framesAfterDamage = 0;
            isDamaging = true;
            spr.color = new Color(1,1,1,1);
        }
    }

    public int HP{ get => hp; }

    ///<summary>全回復</summary>
    public void FullRecover() => ChangeHP(max_hp);

    void Start()
    {
        camera = CameraController.CurrentCamera;
    }

    void Update()
    {
        if(this.isDamaging){

            if(framesAfterDamage<damagemove.GetLength(0)){
                camera.transform.localPosition += new Vector3(damagemove[framesAfterDamage,0],damagemove[framesAfterDamage,1]);

            }else if(framesAfterDamage==damagemove.GetLength(0)){
                camera.transform.localPosition = HeroDefiner.CurrentHeroPastPos[0] + new Vector3(0,50,-200); 
                //短時間に複数回被弾したときに画面揺れの途中で揺れの状態が初めに戻って二重にずれてる、応急処置
            }

            if(framesAfterDamage < framesAfterRecover){
                if(framesAfterDamage==19 || framesAfterDamage==20){ //これはなぜ
                    if(HP==0)spr.sprite = img0;
                    else if(HP==1)spr.sprite = img1;
                    else if(HP==2)spr.sprite = img2;

                }else if(framesAfterDamage==60){
                    spr.color = new Color(1,1,1,180f/255f);
                    isDamaging = false;
                }
            }
        }

        framesAfterDamage ++;

        if(framesAfterRecover < framesAfterDamage){

            if (framesAfterRecover == 40){
                if(HP==2)      spr.sprite = img2;
                else if(HP==3) spr.sprite = img3;

            }else if(framesAfterRecover == 60){
                spr.color = new Color(1,1,1,180f/255f);
            }
        }

        framesAfterRecover ++;
    }
}
