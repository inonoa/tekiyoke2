using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PauseUIMover : MonoBehaviour
{
    [SerializeField] int selected = 0; //0:続ける 1:?? 2:やめる
    [SerializeField] Image draftName;
    [SerializeField] Image pausePause;
    [SerializeField] Image options;
    [SerializeField] Image mark;
    RectTransform markRTF;
    SoundGroup soundGroup;
    float alphaDelta = 1/(float)moveFrames;
    float[] moveDists = new float[moveFrames];
    static readonly int moveFrames = 15;
    static readonly float totalMoveDist = 40;
    int framesFromStart = 0;
    public event EventHandler pauseEnd;

    IAskedInput input;

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

    void Start()
    {
        for(int i=0;i<moveFrames;i++){
            moveDists[i] = ( totalMoveDist - totalMoveDist * (i+1 - moveFrames)*(i+1 - moveFrames) / (float)moveFrames / (float)moveFrames )
                            - ( totalMoveDist - totalMoveDist * (i - moveFrames)*(i - moveFrames) / (float)moveFrames / (float)moveFrames );
        }
        markRTF = mark.GetComponent<RectTransform>();
        soundGroup = GetComponent<SoundGroup>();
        input = ServicesLocator.Instance.GetInput();
    }

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
        if(input.GetButtonDown(ButtonCode.Down)){
            if(selected<2){
                selected ++;
                mark.transform.localPosition += new Vector3(-40,-127);
                soundGroup.Play("Move");
            }
        }
        if(input.GetButtonDown(ButtonCode.Up)){
            if(selected>0){
                selected --;
                mark.transform.localPosition -= new Vector3(-40,-127);
                soundGroup.Play("Move");
            }
        }
        if(input.GetButtonDown(ButtonCode.Enter)){
            soundGroup.Play("Enter");
            if(selected==0){
                Reset();
                pauseEnd?.Invoke(this,EventArgs.Empty);
            }else if(selected==2){
                SceneTransition.Start2ChangeScene("StageChoiceScene",SceneTransition.TransitionType.Default);
            }
        }
    }
}
