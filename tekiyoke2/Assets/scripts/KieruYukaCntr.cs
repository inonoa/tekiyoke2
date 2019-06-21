using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class YukaInfo{
    public GameObject obj;
    public int frames;
    public bool kieta;

    public YukaInfo(GameObject obj){
        this.obj = obj;
        frames = 40;
        kieta = false;
    }
}

public class KieruYukaCntr : MonoBehaviour
{
    private List<YukaInfo> infos = new List<YukaInfo>();

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
        for(int i=infos.Count-1;i>-1;i--){
            infos[i].frames  --;
            if(infos[i].kieta){
                if(infos[i].frames==0){
                    infos[i].obj.SetActive(true);
                    infos[i].obj.GetComponent<SpriteRenderer>().sprite = kienaiSprite;
                    infos.Remove(infos[i]);
                }
            }else{
                if(infos[i].frames==25){
                    infos[i].obj.GetComponent<SpriteRenderer>().sprite = kieruSprite;
                }
                else if(infos[i].frames==5){
                    infos[i].obj.GetComponent<SpriteRenderer>().sprite = kieteruSprite;
                }
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
