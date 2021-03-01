using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UniRx;
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

    public IObservable<Texture2D> OnEnd => _onEnd;
    Subject<Texture2D> _onEnd = new Subject<Texture2D>();
    
    public string NextSceneName{ get; set; } = "";

    Texture2D scsho = null;

    void Start()
    {
        //ステージのスクショを撮る(まだ表示してない)
        DOVirtual.DelayedCall(1f, () =>
        {
            CameraController.Current.ScShoOutOfWind(ss => scsho = ss);
        });

        StartCoroutine(PlayWindSound());

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
            CameraController.Current.ScSho(_onEnd.OnNext);
        });
    }

    IEnumerator PlayWindSound(){
        //StartでやるとSEの準備より先に呼ばれてぬるぽが出る
        yield return new WaitForEndOfFrame();
        GetComponent<SoundGroup>().Play("Wind");
    }
}
