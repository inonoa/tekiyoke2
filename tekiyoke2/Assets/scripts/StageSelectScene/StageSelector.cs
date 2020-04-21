﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour
{   
    #region Objects
    public GameObject draftselect;
    public SpriteRenderer dsRenderer;
    public GameObject waku;
    public SpriteRenderer wakuRenderer;
    public GameObject[] stages;
    public SpriteRenderer[] stRenderer;

    public SpriteRenderer bg;

    //クロスフェード用に背後に映すやつ
    public SpriteRenderer bgbg;

    public SpriteRenderer anmaku;

    public Sprite[] bgs;
    public WakuLightMover wakuLight;

    #endregion

    #region States
    enum State{
        Entering, WakuAppearing, Active, Selected
    }
    State state = State.Entering;
    public int selected = 1;

    #endregion

    void Start()
    {
        stRenderer = new SpriteRenderer[stages.Length];
        for(int i=0;i<stRenderer.Length;i++){
            stRenderer[i] = stages[i].GetComponent<SpriteRenderer>();
        }
        dsRenderer = draftselect.GetComponent<SpriteRenderer>();
        wakuRenderer = waku.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //選択したステージのUIに近づく
        Vector3 vv = stages[selected-1].transform.position - waku.transform.position;

        //枠の移動
        if(vv.x*vv.x+vv.y*vv.y<5){
            waku.transform.position = new Vector3(stages[selected-1].transform.position.x,stages[selected-1].transform.position.y,-2);
        }else{
            int signX = 1; int signY = 1; if(vv.x<0){signX = -1;} if(vv.y<0){signY = -1;} //Sqrtに負の数は渡せない(それはそう)
            vv = 2* new Vector3(signX *(float)System.Math.Sqrt(vv.x * signX), signY *(float)System.Math.Sqrt(vv.y * signY),0);

            waku.transform.position += vv;
        }

        //背景のクロスフェード
        if(bg.color.a<1){
            bg.color -= new Color(0,0,0,0.05f);
            anmaku.color = new Color(1,1,1,System.Math.Min(bg.color.a,1-bg.color.a));
            if(bg.color.a<=0){
                bg.sprite = bgbg.sprite;
                bg.color = new Color(1,1,1,1);
            }
        }

        switch(state){
            case State.Entering:
                //手作業での位置調整で厳しい(不透明度を徐々に上げていきある程度上がったら次フェイズへ)
                const float targetAlpha = 0.7f;
                for(int i=0;i<stages.Length;i++){
                    stRenderer[i].color += new Color(0,0,0,targetAlpha * 0.1f);
                    stages[i].transform.position -= new Vector3((float)System.Math.Sqrt(stages[i].transform.position.x),0,0);
                }
                dsRenderer.color += new Color(0,0,0,0.1f);

                if(stRenderer[0].color.a >= targetAlpha){
                    state = State.WakuAppearing;
    
                    for(int i=0;i<stages.Length;i++){
                        stRenderer[i].color = new Color(stRenderer[i].color.r,stRenderer[i].color.g,stRenderer[i].color.b,targetAlpha);
                        stages[i].transform.position = new Vector3(0,stages[i].transform.position.y,stages[i].transform.position.z);
                    }
                }
                break;
    
            case State.WakuAppearing:
                wakuRenderer.color += new Color(0,0,0,0.1f);
                if(wakuRenderer.color.a>=1){
                    state = State.Active;
                }
                break;
    
            case State.Active:
                if(InputManager.Instance.GetButtonDown(ButtonCode.Up)){
                    if(selected>1){
                        selected--;
                        bgbg.sprite = bgs[selected-1];
                        bg.color = new Color(1,1,1,0.99f);
                    }
                }
                if(InputManager.Instance.GetButtonDown(ButtonCode.Down)){
                    if(selected<3){
                        selected++;
                        bgbg.sprite = bgs[selected-1];
                        bg.color = new Color(1,1,1,0.99f);
                    }
                }
                if(InputManager.Instance.GetButtonDown(ButtonCode.Enter)){
                    if(selected==1){
                        state = State.Selected;
                        wakuLight.Stop();
                        SceneTransition.Start2ChangeState("Draft1",SceneTransition.TransitionType.Normal);
                    }
                }
                break;

            case State.Selected:
                stRenderer[selected-1].color += new Color(0,0,0,0.05f);
                break;
        }
            
    }   
}   
