using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    static SceneTransition currentInstance;

    [SerializeField]
    GameObject curtainIn;
    [SerializeField]
    GameObject curtainOut;

    enum SceneTransitState{
        None, Normal
    }
    static SceneTransitState state = SceneTransitState.None;

    ///<summary>様々な遷移がある</summary>
    public enum TransitionType{
        Default, Normal
    }

    public static void Start2ChangeState(string sceneName, TransitionType transitionType){
        switch(transitionType){
            case TransitionType.Default:
                //
                break;
            case TransitionType.Normal:
                Instantiate(currentInstance.curtainIn, currentInstance.transform);
                break;
        }
    }

    void Start(){
        currentInstance = this;
    }
}
