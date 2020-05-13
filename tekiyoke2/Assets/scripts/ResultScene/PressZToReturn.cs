using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PressZToReturn : MonoBehaviour
{
    IAskedInput input;
    [SerializeField] CursorMove cursor;
    bool canPress = false;
    void Start(){
        DOVirtual.DelayedCall(1f, () => canPress = true);
        input = ServicesLocator.Instance.GetInput();
    }

    void Update()
    {
        if(input.GetButtonDown(ButtonCode.Enter) && canPress){
            SceneTransition.Start2ChangeScene("StageChoiceScene", SceneTransition.TransitionType.Normal);
            cursor.OnPushed();
            GetComponent<SoundGroup>().Play("Put");
        }
    }
}
