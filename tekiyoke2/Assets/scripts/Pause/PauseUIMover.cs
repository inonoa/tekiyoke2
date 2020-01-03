using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PauseUIMover : MonoBehaviour
{
    public int selected = 0; //0:続ける 1:?? 2:やめる
    public Image draftName;
    public Image pausePause;
    public Image options;
    public Image mark;
    RectTransform markRTF;
    float alphaDelta = 1/(float)moveFrames;
    public float[] moveDists = new float[moveFrames];
    static readonly int moveFrames = 15;
    static readonly float totalMoveDist = 40;
    int framesFromStart = 0;
    public event EventHandler pauseEnd;

    public void Reset(){
        draftName.transform.localPosition -= new Vector3(totalMoveDist,0);
        draftName.color = new Color(1,1,1,0);
        pausePause.transform.localPosition -= new Vector3(-totalMoveDist,0);
        pausePause.color = new Color(1,1,1,0);
        options.transform.localPosition -= new Vector3(totalMoveDist,0);
        options.color = new Color(1,1,1,0);
        mark.transform.localPosition -= new Vector3(totalMoveDist,0);
        mark.color = new Color(1,1,1,0);
        framesFromStart = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<moveFrames;i++){
            moveDists[i] = ( totalMoveDist - totalMoveDist * (i+1 - moveFrames)*(i+1 - moveFrames) / (float)moveFrames / (float)moveFrames )
                            - ( totalMoveDist - totalMoveDist * (i - moveFrames)*(i - moveFrames) / (float)moveFrames / (float)moveFrames );
        }
        markRTF = mark.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(framesFromStart<moveFrames){
            draftName.transform.localPosition += new Vector3(moveDists[framesFromStart],0);
            draftName.color = new Color(1,1,1, (framesFromStart+1) * alphaDelta);
            pausePause.transform.localPosition += new Vector3(-moveDists[framesFromStart],0);
            pausePause.color = new Color(1,1,1, (framesFromStart+1) * alphaDelta);
            options.transform.localPosition += new Vector3(moveDists[framesFromStart],0);
            options.color = new Color(1,1,1, (framesFromStart+1) * alphaDelta);
            mark.transform.localPosition += new Vector3(moveDists[framesFromStart],0);
            mark.color = new Color(1,1,1, (framesFromStart+1) * alphaDelta);

            framesFromStart ++;
        }
        markRTF.Rotate(new Vector3(0,0,3));
        if(InputManager.Instance.GetButtonDown(ButtonCode.Down)){
            if(selected<2){
                selected ++;
                mark.transform.localPosition += new Vector3(-40,-127);
            }
        }
        if(InputManager.Instance.GetButtonDown(ButtonCode.Up)){
            if(selected>0){
                selected --;
                mark.transform.localPosition -= new Vector3(-40,-127);
            }
        }
        if(InputManager.Instance.GetButtonDown(ButtonCode.Enter)){
            if(selected==0){
                Reset();
                pauseEnd?.Invoke(this,EventArgs.Empty);
            }else if(selected==2){
                SceneTransition.Start2ChangeState("StageChoiceScene",SceneTransition.TransitionType.Default);
            }
        }
    }
}
