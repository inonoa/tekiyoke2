using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour
{   
    #region Objects
    public GameObject draftselect;
    public GameObject waku;
    public GameObject[] stages;

    public GameObject curtain;

    public SpriteRenderer bg;

    //クロスフェード用に背後に映すやつ
    public SpriteRenderer bgbg;

    public Sprite[] bgs;

    #endregion

    #region States
    enum State{
        Entering, WakuAppearing, Active, Selected
    }
    State state = State.Entering;
    public int selected = 1;

    #endregion

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
            waku.transform.position = new Vector3(stages[selected-1].transform.position.x,stages[selected-1].transform.position.y,-2);
        }else{
            int signX = 1; int signY = 1; if(vv.x<0){signX = -1;} if(vv.y<0){signY = -1;} //Sqrtに負の数は渡せない(それはそう)
            vv = 2* new Vector3(signX *(float)System.Math.Sqrt(vv.x * signX), signY *(float)System.Math.Sqrt(vv.y * signY),0);

            waku.transform.position += vv;
        }

        if(bg.color.a<1){
            bg.color -= new Color(0,0,0,0.05f);
            if(bg.color.a<=0){
                bg.sprite = bgbg.sprite;
                bg.color = new Color(1,1,1,1);
            }
        }

        if(state==State.Entering){
            //手作業での位置調整で厳しい
            foreach(GameObject st in stages){
                st.GetComponent<SpriteRenderer>().color += new Color(0,0,0,0.06f);
                st.transform.position -= new Vector3((float)System.Math.Sqrt(st.transform.position.x),0,0);
            }
            draftselect.GetComponent<SpriteRenderer>().color += new Color(0,0,0,0.1f);
            if(stages[0].GetComponent<SpriteRenderer>().color.a>=0.6f){
                state = State.WakuAppearing;

                foreach(GameObject st in stages){
                    st.GetComponent<SpriteRenderer>().color = new Color(st.GetComponent<SpriteRenderer>().color.r,
                        st.GetComponent<SpriteRenderer>().color.g,st.GetComponent<SpriteRenderer>().color.b,0.6f);
                    st.transform.position = new Vector3(0,st.transform.position.y,st.transform.position.z);
                }
            }

        }else if(state==State.WakuAppearing){
            waku.GetComponent<SpriteRenderer>().color += new Color(0,0,0,0.1f);
            if(waku.GetComponent<SpriteRenderer>().color.a>=1){
                state = State.Active;
            }

        }else if(state==State.Active){
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                if(selected>1){
                    selected--;
                    bgbg.sprite = bgs[selected-1];
                    bg.color = new Color(1,1,1,0.99f);
                }
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)){
                if(selected<3){
                    selected++;
                    bgbg.sprite = bgs[selected-1];
                    bg.color = new Color(1,1,1,0.99f);
                }
            }
            if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)){
                if(selected==1){
                    curtain.SetActive(true);
                }
            }
        }
    }
}
