using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScSho : MonoBehaviour
{
    public Camera mainCamera;
    public Camera pauseCamera;
    public Image scshoImg;
    public GameObject pauseMaster;
    public GameObject gameMaster;
    Texture2D texture;
    private int PauseFlag = 0;
    static readonly int pauseTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) && PauseFlag==0){
            StartCoroutine("TakeScSho");
        }
        else if(PauseFlag>0){
            PauseFlag --;
            if(PauseFlag==0){
                scshoImg.sprite = Sprite.Create(texture,new Rect(0, 0, Screen.width, Screen.height),new Vector2(0.5f,0.5f));
                scshoImg.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                gameMaster.SetActive(false);
                pauseMaster.SetActive(true);
            }
        }
    }

    IEnumerator TakeScSho(){
        yield return new WaitForEndOfFrame();

        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
        PauseFlag = pauseTime;
    }
}
