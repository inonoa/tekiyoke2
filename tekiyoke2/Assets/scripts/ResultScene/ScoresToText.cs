using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresToText : MonoBehaviour
{
    [SerializeField] Text timeTxt;

    void Start()
    {
        int stgIdx = SceneTransition.LastStageIndex();
        float time;
        if(stgIdx==-1) time = 69.86f;
        else           time = ScoreHolder.Instance.clearTimesLast[stgIdx];

        int minutes = ((int) time) / 60;
        int seconds = ((int) time) % 60;
        int comma__ = (int) ((time - (int) time) * 100);
        timeTxt.text = "time: " + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + comma__.ToString("00");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
