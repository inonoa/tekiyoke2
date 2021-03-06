using System;
using PlayFab;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayFabLoginManager", menuName = "ScriptableObject/PlayFab Login Manager", order = 0)]
public class PlayFabLoginManager : SerializedScriptableObject
{
    [SerializeField] IPlayFabLogin login;

    IObservable<Unit>         onCurrentLoginSuccess;
    IObservable<PlayFabError> onCurrentLoginError;

    public void Login(Action onSuccess, Action<PlayFabError> onError)
    {
        if (IsLoggedIn())
        {
            onSuccess?.Invoke();
            return;
        }
        
        if (onCurrentLoginSuccess is null)
        {
            var onSuccessSubj = new Subject<Unit>();
            var onErrorSubj = new Subject<PlayFabError>();
            onCurrentLoginSuccess = onSuccessSubj;
            onCurrentLoginError = onErrorSubj;
            
            login.Login
            (
                () =>
                {
                    onSuccessSubj.OnNext(Unit.Default);
                    onCurrentLoginSuccess = null;
                    onCurrentLoginError = null;
                },
                error =>
                {
                    onErrorSubj.OnNext(error);
                    onCurrentLoginSuccess = null;
                    onCurrentLoginError = null;
                }
            );
        }
        
        onCurrentLoginSuccess.Subscribe(_ => onSuccess?.Invoke());
        onCurrentLoginError.Subscribe(error => onError?.Invoke(error));
    }

    public bool IsLoggedIn() => PlayFabClientAPI.IsClientLoggedIn();
}
