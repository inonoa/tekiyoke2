using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UniRx;

public class Pauser : MonoBehaviour
{

    ///<summary>ポーズ画面に貼り付けるスクショ</summary>
    [SerializeField] Image scshoImg;

    //ポーズ/ゲーム切り替える親玉
    [SerializeField] GameObject pauseMaster;
    [SerializeField] GameObject gameMaster;

    ///<summary>いまポーズ中？</summary>
    bool inPause = false;

    PauseView _view;

    SoundGroup soundGroup;
    IAskedInput input;

    Subject<Unit> _OnPause    = new Subject<Unit>();
    Subject<Unit> _OnPauseEnd = new Subject<Unit>();

    public IObservable<Unit> OnPause    => _OnPause;
    public IObservable<Unit> OnPauseEnd => _OnPauseEnd;

    public static Pauser Instance{ get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _view    = pauseMaster.GetComponent<PauseView>();
        soundGroup = GetComponent<SoundGroup>();

        _view.pauseEnd += PauseEnded;

        input = ServicesLocator.Instance.GetInput();
    }

    void Update()
    {
        // 押したら画面切り替え
        if(input.GetButtonDown(ButtonCode.Pause) && !inPause)
        {
            Pause();
        }
    }

    public void Pause()
    {
        //(実際にはフレーム終了後に移行)
        CameraController.CurrentCamera.ScSho(ss =>
        {
            scshoImg.sprite = Sprite.Create
            (
                ss,
                new Rect(0, 0, Screen.width, Screen.height),
                new Vector2(0.5f, 0.5f)
            );

            gameMaster.SetActive(false);
            pauseMaster.SetActive(true);

            //フラグ？書き換え
            inPause = true;
        });

        soundGroup.Play("Pause");

        _OnPause.OnNext(Unit.Default);
    }

    public void PauseEnded(System.Object sender, EventArgs e)
    {
        gameMaster.SetActive(true);
        pauseMaster.SetActive(false);
        inPause = false;
        _OnPauseEnd.OnNext(Unit.Default);
    }
}
