using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PressZToReturn : MonoBehaviour
{
    [SerializeField] CursorMove cursor;
    bool canPress = false;
    void Start() => DOVirtual.DelayedCall(1f, () => canPress = true);
    void Update()
    {
        if(InputManager.Instance.GetButtonDown(ButtonCode.Enter) && canPress){
            SceneTransition.Start2ChangeState("StageChoiceScene", SceneTransition.TransitionType.Normal);
            cursor.OnPushed();
        }
    }
}
