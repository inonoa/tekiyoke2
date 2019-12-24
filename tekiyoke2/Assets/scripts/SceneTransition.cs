using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    static SceneTransition currentInstance;

    [SerializeField]
    GameObject curtain4SceneEnd;
    [SerializeField]
    GameObject curtain4SceneStart;

    enum SceneTransitState{ None, Default, Normal }
    static SceneTransitState _State = SceneTransitState.Normal;
    static SceneTransitState State{
        get{ return _State; }
        set{ _State = value; Debug.Log(value); }
    }

    ///<summary>様々な遷移がある</summary>
    public enum TransitionType{
        ///<summary>素のLoadScene()が呼ばれる</summary>
        Default,
        ///<summary>今のとこカーテンが出て横にシューっとなる(？)、大体の場合これを使うみたいな感じで</summary>
        Normal
    }

    public static void Start2ChangeState(string sceneName, TransitionType transitionType){
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
        }
    }

    void Start(){
        Debug.Log(SceneTransition.State);
        currentInstance = this;

        switch(SceneTransition.State){
            case SceneTransitState.None:
                break;

            case SceneTransitState.Default:
                break;
            
            case SceneTransitState.Normal:
                Instantiate(curtain4SceneStart, transform);
                break;
        }

        //ステート名からくる直感に反するのでアレ
        SceneTransition.State = SceneTransitState.None;
    }
}
