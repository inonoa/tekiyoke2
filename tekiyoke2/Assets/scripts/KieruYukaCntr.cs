using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

///<summary>消える床とその消えるまでの時間/消えてからの時間などを持っておく、KieruYukaCntrで監視する</summary>
class YukaInfo{
    public GameObject obj;
    ///<summary>消えるまでの時間/消えてからの時間</summary>
    public int frames;
    ///<summary>消えていたらtrue</summary>
    public bool kieta;

    public YukaInfo(GameObject obj){
        this.obj = obj;
        frames = 40;
        kieta = false;
    }
}

public class KieruYukaCntr : MonoBehaviour
{
    ///<summary>ここに床(YukaInfo)を入れると消してくれたり再出現させてくれたりする</summary>
    private List<YukaInfo> infos = new List<YukaInfo>();

    ///<summary>消える床に主人公が乗ったときその床(sender(KieruYukaクラス))を監視対象に追加する(既に監視対象ならやめる)</summary>
    public void AddYuka(object sender, EventArgs e){
        KieruYuka yuka = (KieruYuka)sender;
        foreach(YukaInfo y in infos){
            if(y.obj==yuka.gameObject){
                return;
            }
        }
        infos.Add(new YukaInfo(yuka.gameObject));
    }

    public HeroMover hero;
    public Sprite kieruSprite;
    public Sprite kieteruSprite;
    public Sprite kienaiSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //一定時間でスプライトを変える、消える、現れる
        for(int i=infos.Count-1;i>-1;i--){
            infos[i].frames  --;
            if(infos[i].kieta){
                ///<summary>再出現</summary>
                if(infos[i].frames==0){
                    infos[i].obj.SetActive(true);
                    infos[i].obj.GetComponent<SpriteRenderer>().sprite = kienaiSprite;
                    infos.Remove(infos[i]);
                }
            }else{
                ///<summary>スプライト変更</summary>
                if(infos[i].frames==25){
                    infos[i].obj.GetComponent<SpriteRenderer>().sprite = kieruSprite;
                }
                ///<summary>スプライト変更</summary>
                else if(infos[i].frames==5){
                    infos[i].obj.GetComponent<SpriteRenderer>().sprite = kieteruSprite;
                }
                ///<summary>消える</summary>
                else if(infos[i].frames==0){
                    infos[i].obj.SetActive(false);
                    infos[i].frames = 100;
                    infos[i].kieta = true;
                    hero.isOnGround = false;
                }
            }
        }
    }
}
