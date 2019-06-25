using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Pauser : MonoBehaviour
{

    ///<summary>ポーズ画面に貼り付けるスクショ</summary>
    public Image scshoImg;

    //ポーズ/ゲーム切り替える親玉
    public GameObject pauseMaster;
    public GameObject gameMaster;
    
    ///<summary>スクショ(ポーズに入るときに撮って背景に映す)</summary>
    Texture2D scsho;

    ///<summary>いまポーズ中？</summary>
    bool inPause = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 押したら画面切り替え
        if(Input.GetKeyDown(KeyCode.S)){

            //ポーズに移行(実際にはフレーム終了後に移行)
            if(!inPause){ StartCoroutine("TakeScSho"); Debug.Log("ポーズ開始！"); }

            //ポーズ終了
            else{
                Debug.Log("ポーズ終了！");
                gameMaster.SetActive(true);
                pauseMaster.SetActive(false);
                inPause = false;
            }
        }
    }

    ///<summary>ボタンを押したときに呼ばれる、フレーム終了時にスクショを撮り、ポーズに移行</summary>
    IEnumerator TakeScSho(){
        //フレーム終了まで待つ
        yield return new WaitForEndOfFrame();

        //スクショ
        scsho = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        scsho.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        scsho.Apply();

        //ポーズ画面の背景にスクショをセット
        scshoImg.sprite = Sprite.Create(scsho,new Rect(0, 0, Screen.width, Screen.height),new Vector2(0.5f,0.5f));
        scshoImg.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        //ポーズに切り替え
        gameMaster.SetActive(false);
        pauseMaster.SetActive(true);

        //フラグ？書き換え
        inPause = true;
    }
}
