using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;

public class Tweeter : SerializedMonoBehaviour
{

    [SerializeField, Tooltip("`[score]`でスコアが入るよ"), Multiline]
    string tweetText;
    string url;
    [SerializeField] IAskedInput input;

    [Space(10), SerializeField] ScoreHolder scoreHolder;

#if UNITY_WEBGL
    [DllImport("__Internal")] private static extern void OpenNewWindow(string url);
#endif

    void Start()
    {
        int stageIdx = SceneTransition.LastStageIndex();
        float time = stageIdx != -1 ? scoreHolder.Get().Time : 69.865f;
        string actualTweetText = tweetText.Replace(
            "[score]",
            time.ToTimeString()
        );

        url = "https://twitter.com/intent/tweet?text=" 
            + UnityWebRequest.EscapeURL(actualTweetText);
    }

    void Update()
    {
        if(input.GetButtonDown(ButtonCode.Tweet)){
            OpenTweetWindow();
        }
    }

    void OpenTweetWindow(){

#if UNITY_WEBGL && !UNITY_EDITOR
        OpenNewWindow(url);
#else
        Application.OpenURL(url);
#endif
    }
}
