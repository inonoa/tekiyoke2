using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

//風のないスクショを撮ってぼかす、青がスライドインして来たら全体のスクショを取ってResultSceneへ？
//遷移のタイミングが非自明すぎる
public class WindAndBlur : MonoBehaviour
{
    [SerializeField] BlurTrans blur = null;
    [SerializeField] BlueSlideIn blue = null;
    [SerializeField] float secondsToBlur = 3;
    [SerializeField] float secondsToBeReadyToChange = 5;
    event EventHandler scShoTaken;
    public event EventHandler ReadyToChange;
    public string NextSceneName{ get; set; } = "";

    Texture2D scsho = null;

    void Start()
    {
        //ステージのスクショを撮る(まだ表示してない)
        CameraController.CurrentCamera.ScShoOutOfUI(ss => scsho = ss);

        DOVirtual.DelayedCall(secondsToBlur, () =>
        {
            Instantiate(blue, transform).transform.SetAsFirstSibling();
            //ステージのスクショをInstantiateして表示(ぼかされる)
            var blurInst = Instantiate(blur, transform);
            blurInst.GetComponent<Image>().sprite = 
                Sprite.Create(scsho, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f,0.5f));
            blurInst.transform.SetAsFirstSibling();
        });

        DOVirtual.DelayedCall(secondsToBeReadyToChange, () => 
        {
            CameraController.CurrentCamera.ScSho(ss => {
                ReadyToChange?.Invoke(ss, EventArgs.Empty);
                SceneManager.LoadScene(NextSceneName);
            });
        });
    }
}
