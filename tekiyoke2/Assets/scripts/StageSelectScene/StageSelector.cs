using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour
{
    public Stage1 s1;
    public Stage2 s2;
    public Stage3 s3;
    private int selected = 0;
    public int Selected{
        get{return selected;}
        set{
            switch(value){
                case 1:
                    s1.IsSelected = true;
                    s2.IsSelected = false;
                    s3.IsSelected = false;
                    break;
                case 2:
                    s1.IsSelected = false;
                    s2.IsSelected = true;
                    s3.IsSelected = false;
                    break;
                case 3:
                    s1.IsSelected = false;
                    s2.IsSelected = false;
                    s3.IsSelected = true;
                    break;
            }
            selected = value;
        }
    }

    private int openCount = 30;

    public GameObject curtain;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(openCount>0){
            openCount --;
            if(openCount<=0){
                Selected = 1;
            }
        }else{
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                if(Selected==2)Selected = 1;
                else if(Selected==3)Selected = 2;
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)){
                if(Selected==1)Selected = 2;
                else if(Selected==2)Selected = 3;
            }
            if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)){
                curtain.SetActive(true);
            }
        }
    }
}
