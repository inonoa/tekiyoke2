using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    static SceneTransition currentInstance;

    [SerializeField]
    GameObject curtain4SceneEnd = null;
    [SerializeField]
    GameObject curtain4SceneStart = null;

    enum SceneTransitState{ None, Default, Normal, HeroDied }
    static SceneTransitState _State = SceneTransitState.Normal;
    static SceneTransitState State{
        get{ return _State; }
        set{ _State = value; }
    }

    ///<summary>シーンごとにデフォルトのStateを持っておき、そのシーンが初めに呼ばれたらstaticのstateに反映</summary>
    [SerializeField]
    SceneTransitState firstState = SceneTransitState.None;
    ///<summary>firstState参照</summary>
    static bool firstSceneLoaded = false;

    ///<summary>様々な遷移がある</summary>
    public enum TransitionType{
        ///<summary>素のLoadScene()が呼ばれる</summary>
        Default,
        ///<summary>今のとこカーテンが出て横にシューっとなる(？)、大体の場合これを使うみたいな感じで</summary>
        Normal,
        ///<summary>主人公が死んだとき専用の遷移</summary>
        HeroDied
    }

    ///<summary>シーンを変えることを試みる、短時間に複数回遷移させるみたいなことにならないようによしなにする</summary>
    public static void Start2ChangeState(string sceneName, TransitionType transitionType){
        if(SceneTransition.State != SceneTransitState.None) return;

        switch(transitionType){

            case TransitionType.Default:
                SceneTransition.State = SceneTransitState.Default;
                SceneManager.LoadScene(sceneName);
                break;

            case TransitionType.Normal:
                SceneTransition.State = SceneTransitState.Normal;
                var curtain = Instantiate(currentInstance.curtain4SceneEnd, currentInstance.transform);
                curtain.GetComponent<Curtain4SceneEndMover>().NextSceneName = sceneName;
                break;
            
            case TransitionType.HeroDied:
                SceneTransition.State = SceneTransitState.HeroDied;
                var curtainD = Instantiate(currentInstance.curtain4SceneEnd, currentInstance.transform);
                curtainD.GetComponent<Curtain4SceneEndMover>().NextSceneName = sceneName;
                break;
        }
    }

    ///<summary>遷移してきたなら遷移のタイプによって相応のオブジェクトを出す、そうでないならfirstStateを反映</summary>
    void Start(){
        currentInstance = this;
        if(!firstSceneLoaded){
            SceneTransition.State = firstState;
            firstSceneLoaded = true;
        }

        switch(SceneTransition.State){
            case SceneTransitState.None:
                break;

            case SceneTransitState.Default:
                break;
            
            case SceneTransitState.Normal:
                Instantiate(curtain4SceneStart, transform);
                break;
            
            case SceneTransitState.HeroDied:
                Instantiate(curtain4SceneStart, transform);
                MemoryOverDeath.Instance.Load();
                break;
        }

        //ステート名からくる直感に反するのでアレ
        SceneTransition.State = SceneTransitState.None;
    }
}
