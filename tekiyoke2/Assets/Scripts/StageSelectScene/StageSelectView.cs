using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageSelectView : SerializedMonoBehaviour, IStageSelectView
{   
    #region Objects
    [SerializeField] Image chooseADraftImage;
    [SerializeField] Image wakuImage;
    [SerializeField] WakuLightMover wakuLight;
    [SerializeField] Image[] stageImages;
    [SerializeField] SoundGroup soundGroup;
    
    [SerializeField] Image bg;
    [SerializeField] Image bgbg; //クロスフェード用に背後に映すやつ
    [SerializeField] Image anmaku;
    [SerializeField] Sprite[] bgSprites;

    [SerializeField] Button goToRankingsButton;
    [SerializeField] Button goToConfigButton;

    #endregion

    #region States
    enum State{ Entering, WakuAppearing, Active, Selected }
    State state = State.Entering;
    int selected = 1;
    #endregion

    #region 依存

    [SerializeField] IAskedInput input;

    #endregion
    
    Subject<int> _StageSelected = new Subject<int>();
    public IObservable<int> StageSelected => _StageSelected;
    
    Subject<Unit> _OnGoToConfig = new Subject<Unit>();
    public IObservable<Unit> OnGoToConfig => _OnGoToConfig;
    
    Subject<Unit> _OnGoToRankings = new Subject<Unit>();
    public IObservable<Unit> OnGoToRankings => _OnGoToRankings;

    void Start()
    {
        goToRankingsButton.onClick.AddListener(() =>
        {
            ExitMain();
            _OnGoToRankings.OnNext(Unit.Default);
        });
        
        goToConfigButton.onClick.AddListener(() =>
        {
            ExitMain();
            _OnGoToConfig.OnNext(Unit.Default);
        });

        DOVirtual.DelayedCall(1f, () =>
        {
            goToRankingsButton.gameObject.SetActive(true);
            goToConfigButton.gameObject.SetActive(true);
        });
    }

    void ExitMain()
    {
        gameObject.SetActive(false);
        goToConfigButton.gameObject.SetActive(false);
        goToRankingsButton.gameObject.SetActive(false);
    }

    public void Enter()
    {
        gameObject.SetActive(true);
        goToConfigButton.gameObject.SetActive(true);
        goToRankingsButton.gameObject.SetActive(true);
    }

    void Update()
    {
        //選択したステージのUIに近づく
        Vector3 vv = stageImages[selected-1].transform.position - wakuImage.transform.position;

        //枠の移動
        if(vv.x*vv.x+vv.y*vv.y<5){
            wakuImage.transform.position = new Vector3
            (
                stageImages[selected-1].transform.position.x,
                stageImages[selected-1].transform.position.y,
                -2
            );
        }else{
            int signX = 1; int signY = 1; if(vv.x<0){signX = -1;} if(vv.y<0){signY = -1;} //Sqrtに負の数は渡せない(それはそう)
            vv = 2* new Vector3(signX *(float)System.Math.Sqrt(vv.x * signX), signY *(float)System.Math.Sqrt(vv.y * signY),0);

            wakuImage.transform.position += vv;
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

        switch(state)
        {
            case State.Entering:
                //手作業での位置調整で厳しい(不透明度を徐々に上げていきある程度上がったら次フェイズへ)
                const float targetAlpha = 0.7f;
                foreach (Image stageImage in stageImages)
                {
                    stageImage.color += new Color(0,0,0,targetAlpha * 0.1f);
                    stageImage.transform.position -= new Vector3((float)System.Math.Sqrt(stageImage.transform.position.x),0,0);
                }
                chooseADraftImage.color += new Color(0,0,0,0.1f);

                if(stageImages[0].color.a >= targetAlpha)
                {
                    state = State.WakuAppearing;
                    foreach (Image stageImage in stageImages)
                    {
                        stageImage.color = new Color(stageImage.color.r,stageImage.color.g,stageImage.color.b,targetAlpha);
                        stageImage.transform.position = new Vector3(0,stageImage.transform.position.y,stageImage.transform.position.z);
                    }
                }
                break;
    
            case State.WakuAppearing:
                wakuImage.color += new Color(0,0,0,0.1f);
                if(wakuImage.color.a >= 1)
                {
                    state = State.Active;
                }
                break;
    
            case State.Active:
                if(input.GetButtonDown(ButtonCode.Up))
                {
                    if(selected > 1)
                    {
                        selected--;
                        bgbg.sprite = bgSprites[selected-1];
                        bg.color = new Color(1,1,1,0.99f);
                        soundGroup.Play("Move");
                    }
                }
                if(input.GetButtonDown(ButtonCode.Down))
                {
                    if(selected < 3)
                    {
                        selected++;
                        bgbg.sprite = bgSprites[selected-1];
                        bg.color = new Color(1,1,1,0.99f);
                        soundGroup.Play("Move");
                    }
                }
                if(input.GetButtonDown(ButtonCode.Enter))
                {
                    state = State.Selected;
                    wakuLight.Stop();
                    _StageSelected.OnNext(selected);
                    soundGroup.Play("Enter");
                }
                break;

            case State.Selected:
                stageImages[selected - 1].color += new Color(0,0,0,0.05f);
                break;
        }
            
    }
}
