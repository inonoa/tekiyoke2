using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HpCntr : MonoBehaviour
{
    [SerializeField] Sprite img3; [SerializeField] Sprite img2; [SerializeField] Sprite img1; [SerializeField] Sprite img0;
    [SerializeField] Sprite img3_2; [SerializeField] Sprite img2_1; [SerializeField] Sprite img1_0;
    [SerializeField] Image spr;

    private static int max_hp = 3;
    private bool isDamaging = false;
    private int framesAfterDamage = 0;

    static readonly int mutekiFramesAfterDamage = 100;
    private bool _CanBeDamaged = true;
    public bool CanBeDamaged{
        get => _CanBeDamaged && (framesAfterDamage > mutekiFramesAfterDamage);
        set => _CanBeDamaged = value;
    }

    ///<summary>ここを直接書き換えない</summary>
    public int hp = max_hp;

    public event EventHandler die;
    public event EventHandler damaged;
    private float[,] damagemove = {{20,0},{0,0},{0,0},{-40,10},{0,0},{0,0},{10,-30},{0,0},{0,0},{15,30},{0,0},{0,0},{-5,-10}};

    new CameraController camera;


    ///<summary>HPの増減はすべてここから。 / プロパティにしては重い処理をしている……</summary>
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
    public void FullRecover() => HP = max_hp;

    // Start is called before the first frame update
    void Start()
    {
        camera = CameraController.CurrentCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isDamaging){

            if(framesAfterDamage<damagemove.GetLength(0)){
                camera.transform.localPosition += new Vector3(damagemove[framesAfterDamage,0],damagemove[framesAfterDamage,1]);

            }else if(framesAfterDamage==damagemove.GetLength(0)){
                camera.transform.localPosition = HeroDefiner.CurrentHeroPastPos[0] + new Vector3(0,50,-200); 
                //短時間に複数回被弾したときに画面揺れの途中で揺れの状態が初めに戻って二重にずれてる、応急処置
            }

            if(framesAfterDamage==19 || framesAfterDamage==20){ //これはなぜ
                if(HP==0)spr.sprite = img0;
                else if(HP==1)spr.sprite = img1;
                else if(HP==2)spr.sprite = img2;

            }else if(framesAfterDamage==60){
                spr.color = new Color(1,1,1,180f/255f);
                isDamaging = false;
            }
        }

        framesAfterDamage ++;
    }
}
