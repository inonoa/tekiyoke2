using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

//staticな関数であってほしいとなんとなく思ってこうしたけどcurrentInstance必要なら覆い隠すべきではない気もしてきたね……
public class SceneTransition : MonoBehaviour
{
    static SceneTransition currentInstance;

    [SerializeField] Curtain4SceneEndMover curtain4SceneEnd = null;
    [SerializeField] Curtain4SceneStartMover curtain4SceneStart = null;
    [SerializeField] WindAndBlur windAndBlur = null;
    [SerializeField] Image scshoImage = null;
    static Texture2D scSho = null;
    [SerializeField] Transform bgTransformForScSho;
    [SerializeField] Image whiteOutImage;
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
                SceneTransition.State = SceneTransitState.HeroDied;
                var curtainD = Instantiate(currentInstance.curtain4SceneEnd, currentInstance.transform);
                curtainD.NextSceneName = sceneName;
                break;
            
            case TransitionType.WindAndBlur:
                SceneTransition.State = SceneTransitState.WindAndBlur;
                PostEffectWrapper noise = CameraController.Current.AfterEffects.Find("Noise");
                if(noise!=null) DOTween.To(noise.GetVolume, noise.SetVolume, 0, 1);
                DOVirtual.DelayedCall(1.2f, () =>
                {
                    var windblur = Instantiate(currentInstance.windAndBlur, currentInstance.transform.parent);
                    windblur.NextSceneName = sceneName;
                    windblur.ReadyToChange += (ss, e) => scSho = (Texture2D)ss;
                    windblur.transform.SetAsLastSibling();
                });
                break;
            
            case TransitionType.WhiteOut:
                SceneTransition.State = SceneTransitState.WhiteOut;
                const float duration = 5f;
                
                PostEffectWrapper noise_ = CameraController.Current?.AfterEffects?.Find("Noise");
                if(noise_ != null) DOTween.To(noise_.GetVolume, noise_.SetVolume, 0, duration / 2);

                Material whiteOutMat = currentInstance.whiteOutImage.material;
                whiteOutMat.SetFloat("_Alpha", 0.3f);
                whiteOutMat.SetFloat("_Whiteness", 0.05f);
                whiteOutMat.To("_Alpha", 1, duration / 1.5f);
                whiteOutMat.To("_Whiteness", 0.9f, duration).SetEase(Ease.InOutCubic)
                    .onComplete += () => SceneManager.LoadScene(sceneName);
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
        if(noise!=null) DOTween.To(noise.GetVolume, noise.SetVolume, 1, 1);

        switch(SceneTransition.State)
        {
            case SceneTransitState.None:
                break;

            case SceneTransitState.Default:
                break;
            
            case SceneTransitState.Normal:
                Instantiate(curtain4SceneStart, transform);
                break;
            
            case SceneTransitState.HeroDied:
                Instantiate(curtain4SceneStart, transform);
                break;

            case SceneTransitState.WindAndBlur:
                Image scshoImg = Instantiate(scshoImage, bgTransformForScSho);
                scshoImg.sprite = Sprite.Create(scSho, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f,0.5f));
                break;
            
            case SceneTransitState.WhiteOut:
                float duration = 3;
                Material whiteOutMat = currentInstance.whiteOutImage.material;
                whiteOutMat.SetFloat("_Alpha", 1f);
                whiteOutMat.SetFloat("_Whiteness", 1);
                whiteOutMat.To("_Alpha", 0, duration);
                whiteOutMat.To("_Whiteness", 0.05f, duration).SetEase(Ease.OutCubic);
                break;
        }

        //ステート名からくる直感に反するのでアレ
        SceneTransition.State = SceneTransitState.None;
    }

    ///<summary>Start()時にsetするだけだとポーズとかの(SceneTranitionがシーン内に複数存在する)場合に支障をきたすらしいので</summary>
    void OnEnable() => currentInstance = this;
}
