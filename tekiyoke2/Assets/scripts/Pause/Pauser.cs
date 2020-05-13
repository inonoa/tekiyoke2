using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Pauser : MonoBehaviour
{

    ///<summary>ポーズ画面に貼り付けるスクショ</summary>
    Image scshoImg;

    //ポーズ/ゲーム切り替える親玉
    GameObject pauseMaster;
    GameObject gameMaster;
    
    ///<summary>スクショ(ポーズに入るときに撮って背景に映す)</summary>
    Texture2D scsho;

    ///<summary>いまポーズ中？</summary>
    bool inPause = false;

    PauseUIMover uiMover;

    SoundGroup soundGroup;
    IAskedInput input;

    void Start()
    {
        gameMaster = DraftManager.CurrentInstance.GameMasterTF.gameObject;
        pauseMaster = DraftManager.CurrentInstance.PauseMasterTF.gameObject;
        uiMover = pauseMaster.GetComponent<PauseUIMover>();
        scshoImg = pauseMaster.transform.Find("Canvas").Find("ScSho").GetComponent<Image>();
        soundGroup = GetComponent<SoundGroup>();

        uiMover.pauseEnd += PauseEnded;

        input = ServicesLocator.Instance.GetInput();
    }

    void Update()
    {
        // 押したら画面切り替え
        if(input.GetButtonDown(ButtonCode.Pause)){

            //ポーズに移行(実際にはフレーム終了後に移行)
            if(!inPause){
                CameraController.CurrentCamera.ScSho(ss => {

                    scshoImg.sprite = Sprite.Create(ss, new Rect(0, 0, Screen.width, Screen.height),new Vector2(0.5f,0.5f));

                    gameMaster.SetActive(false);
                    pauseMaster.SetActive(true);

                    //フラグ？書き換え
                    inPause = true;
                });

                soundGroup.Play("Pause");
            }

        }
    }

    public void PauseEnded(System.Object sender, EventArgs e){
        gameMaster.SetActive(true);
        pauseMaster.SetActive(false);
        inPause = false;
    }
}
