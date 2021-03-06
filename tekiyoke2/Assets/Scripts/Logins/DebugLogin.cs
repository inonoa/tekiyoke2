
using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

[Serializable]
public class DebugLogin : IPlayFabLogin
{
    public void Login(Action onSuccess, Action<PlayFabError> onError)
    {
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID
        (
            request,
            _ => onSuccess.Invoke(),
            onError
        );
    }
}
