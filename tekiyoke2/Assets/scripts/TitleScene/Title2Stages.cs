using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>不要じゃね？</summary>
public class Title2Stages : MonoBehaviour
{
    [SerializeField] CloudSpawner clouds;
    [SerializeField] SaveDataManager saveDataManager;
    IAskedInput input;

    bool inSceneTransition = false;

    void Start()
    {
        input = ServicesLocator.Instance.GetInput();
    }
    
    void Update()
    {
        if(inSceneTransition) return;
        
        if(input.AnyButtonDown())
        {
            inSceneTransition = true;
            
            if(saveDataManager.TutorialFinished)
            {
                clouds.state = CloudSpawner.State.Wind;
                GetComponent<SoundGroup>().Play("Push");
            }
            else
            {
                SceneTransition.Start2ChangeScene("Tutorial", SceneTransition.TransitionType.WhiteOut);
            }
        }
    }
}
