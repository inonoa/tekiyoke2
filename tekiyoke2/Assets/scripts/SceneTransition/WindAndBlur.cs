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
        StartCoroutine("TakeScSho");

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
            //風のエフェクトごとスクショを撮って渡し、シーンを変える(渡したスクショは変えた先のシーンで使われる)
            StartCoroutine("TakeScSho");
            scShoTaken += (ss, e) => {
                ReadyToChange?.Invoke(ss, EventArgs.Empty);
                SceneManager.LoadScene(NextSceneName);
            };
        });
    }

    IEnumerator TakeScSho(){
        //フレーム終了まで待つ
        yield return new WaitForEndOfFrame();

        //スクショ
        scsho = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        scsho.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        scsho.Apply();
        scShoTaken?.Invoke(scsho, EventArgs.Empty);
    }
}
