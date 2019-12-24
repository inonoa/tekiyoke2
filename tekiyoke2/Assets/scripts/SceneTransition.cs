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

    enum SceneTransitState{ None, Normal }
    static SceneTransitState state = SceneTransitState.None;

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
                SceneManager.LoadScene(sceneName);
                break;
            case TransitionType.Normal:
                var curtain = Instantiate(currentInstance.curtain4SceneEnd, currentInstance.transform);
                curtain.GetComponent<GoalCurtainMover>().NextSceneName = sceneName;
                break;
        }
    }

    void Start(){
        currentInstance = this;
    }
}
