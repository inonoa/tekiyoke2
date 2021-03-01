using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;

//staticな関数であってほしいとなんとなく思ってこうしたけどcurrentInstance必要なら覆い隠すべきではない気もしてきたね……
public class SceneTransition : SerializedMonoBehaviour
{
    static SceneTransition currentInstance;

    // 遷移を始めるために使うviewと次のシーンで使うviewが別のオブジェクトという罠がある
    // シーンへの依存を無くせば同じにできそう
    // というか最終的に同じにしないと型情報持ってこないといけなくてあれ
    [SerializeField] ISceneTransitionView[] views;

    [SerializeField] Curtain4SceneEndMover curtain4SceneEnd = null;
    [SerializeField] Curtain4SceneStartMover curtain4SceneStart = null;
    static List<string> _SceneNameLog = new List<string>();
    public static IReadOnlyList<string> SceneNameLog => _SceneNameLog;
    public static int LastStageIndex()
    {
        for(int i = SceneNameLog.Count - 1; i > -1; i--)
        {
            switch(SceneNameLog[i])
            {
                case "Draft1": return 0;
                case "Draft2": return 1;
                case "Draft3": return 2;
            }
        }
        return -1;
    }

    enum SceneTransitState{ None, Default, Normal, HeroDied, WindAndBlur, WhiteOut }
    static SceneTransitState _State = SceneTransitState.Normal;
    static SceneTransitState State
    {
        get{ return _State; }
        set{ _State = value; }
    }

    ///<summary>シーンごとにデフォルトのStateを持っておき、そのシーンが初めに呼ばれたらstaticのstateに反映</summary>
    [SerializeField]
    SceneTransitState firstState = SceneTransitState.None;
    ///<summary>firstState参照</summary>
    static bool firstSceneLoaded = false;

    ///<summary>様々な遷移がある</summary>
    public enum TransitionType
    {
        ///<summary>素のLoadScene()が呼ばれる</summary>
        Default,
        ///<summary>今のとこカーテンが出て横にシューっとなる(？)、大体の場合これを使うみたいな感じで</summary>
        Normal,
        ///<summary>主人公が死んだとき専用の遷移</summary>
        HeroDied,
        ///<summary>風みたいなエフェクトを出した後背景をぼかす(？)</summary>
        WindAndBlur,
        ///<summary>チュートリアルからDraft1への移行(ここでする必要ある？)</summary>
        WhiteOut
    }

    public ISceneTransitionView Find<T>() where T : ISceneTransitionView
        => views.First(v => v is T);

    ///<summary>シーンを変えることを試みる、短時間に複数回遷移させるみたいなことにならないようによしなにする</summary>
    public static void Start2ChangeScene(string sceneName, TransitionType transitionType)
    {
        if(SceneTransition.State != SceneTransitState.None) return;

        switch(transitionType)
        {
            case TransitionType.Default:
                SceneTransition.State = SceneTransitState.Default;
                SceneManager.LoadScene(sceneName);
                break;

            case TransitionType.Normal:
                SceneTransition.State = SceneTransitState.Normal;
                var curtain = Instantiate(currentInstance.curtain4SceneEnd, currentInstance.transform);
                curtain.NextSceneName = sceneName;
                break;
            
            case TransitionType.HeroDied:
                // todo
                SceneTransition.State = SceneTransitState.HeroDied;
                currentInstance.Find<HeroDiedTransitionView>().OnTransitionStart(currentInstance)
                    .Subscribe(_ => SceneManager.LoadScene(sceneName));
                break;
            
            case TransitionType.WindAndBlur:
                SceneTransition.State = SceneTransitState.WindAndBlur;
                currentInstance.Find<WindAndBlueTransitionView>().OnTransitionStart(currentInstance)
                    .Subscribe(_ => SceneManager.LoadScene(sceneName));
                break;
            
            case TransitionType.WhiteOut:
                SceneTransition.State = SceneTransitState.WhiteOut;
                currentInstance.Find<WhiteOutTransitionView>().OnTransitionStart(currentInstance)
                    .Subscribe(_ => SceneManager.LoadScene(sceneName));
                break;
        }
    }

    void Awake()
    {
        _SceneNameLog.Add(gameObject.scene.name);
    }

    ///<summary>遷移してきたなら遷移のタイプによって相応のオブジェクトを出す、そうでないならfirstStateを反映</summary>
    void Start()
    {
        currentInstance = this;
        if(!firstSceneLoaded)
        {
            SceneTransition.State = firstState;
            firstSceneLoaded = true;
        }
        
        PostEffectWrapper noise = CameraController.Current?.AfterEffects?.Find("Noise");

        switch(SceneTransition.State)
        {
            case SceneTransitState.None:
                break;

            case SceneTransitState.Default:
                break;
            
            case SceneTransitState.Normal:
                Instantiate(curtain4SceneStart, transform);
                if(noise != null) DOTween.To(noise.GetVolume, noise.SetVolume, 1, 1);
                break;
            
            case SceneTransitState.HeroDied:
                Find<HeroDiedTransitionView>().OnNextSceneStart(this);
                break;

            case SceneTransitState.WindAndBlur:
                Find<WindAndBlueTransitionView>().OnNextSceneStart(this);
                break;
            
            case SceneTransitState.WhiteOut:
                Find<WhiteOutTransitionView>().OnNextSceneStart(this);
                break;
        }

        //ステート名からくる直感に反するのでアレ
        SceneTransition.State = SceneTransitState.None;
    }

    ///<summary>Start()時にsetするだけだとポーズとかの(SceneTranitionがシーン内に複数存在する)場合に支障をきたすらしいので</summary>
    void OnEnable() => currentInstance = this;
}

public interface ISceneTransitionView
{
    IObservable<Unit> OnTransitionStart(SceneTransition sceneTransition);
    void OnNextSceneStart(SceneTransition sceneTransition);
}
