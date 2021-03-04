using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PressZToReturn : SerializedMonoBehaviour
{
    [SerializeField] IInput input;
    [SerializeField] CursorMove cursor;
    bool canPress = false;
    void Start()
    {
        DOVirtual.DelayedCall(1f, () => canPress = true);
    }

    void Update()
    {
        if(input.GetButtonDown(ButtonCode.Enter) && canPress){
            SceneTransition.StartToChangeScene<NormalTransitionView>("StageChoiceScene");
            cursor.OnPushed();
            GetComponent<SoundGroup>().Play("Put");
        }
    }
}
