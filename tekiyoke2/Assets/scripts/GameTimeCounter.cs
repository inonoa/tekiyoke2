using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;

public class GameTimeCounter : MonoBehaviour
{
    float count=0;
    Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = (((int)count)/3600).ToString("00") + ":" + (((int)count)%3600/60).ToString("00") + ":" + (((int)count)%60).ToString("00");
        count += Time.timeScale;
    }
}
