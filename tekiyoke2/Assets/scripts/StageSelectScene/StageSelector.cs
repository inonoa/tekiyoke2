using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour
{   
    #region GameObjects
    public GameObject waku;
    public GameObject[] stages;

    public GameObject curtain;

    #endregion

    enum State{
        Entering, Active, Selected
    }
    State state = State.Active;
    public int selected = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //選択したステージのUIに近づく
        Vector3 vv = stages[selected-1].transform.position - waku.transform.position;

        if(vv.x*vv.x+vv.y*vv.y<5){
            waku.transform.position = stages[selected-1].transform.position;
        }else{
            int signX = 1; int signY = 1; if(vv.x<0){signX = -1;} if(vv.y<0){signY = -1;} //Sqrtに負の数は渡せない(それはそう)
            vv = 2* new Vector3(signX *(float)System.Math.Sqrt(vv.x * signX), signY *(float)System.Math.Sqrt(vv.y * signY),0);

            waku.transform.position += vv;
        }

        if(state==State.Active){
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                if(selected>1)selected--;
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)){
                if(selected<3)selected++;
            }
            if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)){
                if(selected==1){
                    curtain.SetActive(true);
                }
            }
        }
    }
}
