using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;

// staticな関数であってほしいとなんとなく思ってこうしたけどcurrentInstance必要なら覆い隠すべきではない気もしてきたね……
public class SceneTransition : SerializedMonoBehaviour
{
    static SceneTransition currentInstance;

    // 遷移を始めるために使うviewと次のシーンで使うviewが別のオブジェクトという罠がある
    // シーンへの依存を無くせば同じにできそう
    // というか最終的に同じにしないと型情報持ってこないといけなくてあれ
    [SerializeField] ISceneTransitionView[] views;
    
    // 持ってきた………
    static Type currentTransitionType;
    
    static List<string> _SceneNameLog = new List<string>();
    public static IReadOnlyList<string> SceneNameLog => _SceneNameLog;
    public static int LastStageIndex()
    {
        for(int i = SceneNameLog.Count - 1; i > -1; i--)
        {
            switch(SceneNameLog[i])
            {
                case "Draft1": return 0;
                case "Draft2": return 1;
                case "Draft3": return 2;
            }
        }
        return -1;
    }

    [SerializeField, ValueDropdown(nameof(Transitions))] string firstTransition;
    string[] Transitions() => views.Select(view => view.ToString()).ToArray();
    
    ///<summary>firstState参照</summary>
    static bool firstSceneLoaded = false;

    public static void StartToChangeScene<T>(string nextSceneName)
        where T : ISceneTransitionView
    {
        if(currentTransitionType != null) return;
        
        currentTransitionType = typeof(T);
        
        currentInstance.views.First(view => view is T)
            .OnTransitionStart(currentInstance)
            .Subscribe(_ => SceneManager.LoadScene(nextSceneName));
    }

    void Awake()
    {
        _SceneNameLog.Add(gameObject.scene.name);
    }

    ///<summary>遷移してきたなら遷移のタイプによって相応のオブジェクトを出す、そうでないならfirstStateを反映</summary>
    void Start()
    {
        currentInstance = this;

        Type transType = firstSceneLoaded ? currentTransitionType : Type.GetType(firstTransition);

        // 1シーンに複数インスタンスがあるケース…………(無くしたい)
        if(transType is null) return;

        views.First(view => view.GetType() == transType)
            .OnNextSceneStart(this);

        currentTransitionType = null;
        firstSceneLoaded = true;
    }

    ///<summary>Start()時にsetするだけだとポーズとかの(SceneTranitionがシーン内に複数存在する)場合に支障をきたすらしいので</summary>
    void OnEnable() => currentInstance = this;
}

public interface ISceneTransitionView
{
    IObservable<Unit> OnTransitionStart(SceneTransition sceneTransition);
    void OnNextSceneStart(SceneTransition sceneTransition);
}
