using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;

[CreateAssetMenu(fileName = "PlayFab Saver", menuName = "Scriptable Object/PlayFabSaver")]
public class PlayFabSaver : ScriptableObject, IDataSaver
{
    public void Save(SaveData data)
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            Save_(data);
        }
        else
        {
            Login(result => Save_(data));
        }

        void Save_(SaveData data_)
        {
            var request = new UpdateUserDataRequest
            {
                Data = data_.ToDictionary()
            };
            PlayFabClientAPI.UpdateUserData
            (
                request, 
                result => {},
                error  => {}
            );
            
            var nameRequest = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = data_.playerName
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName
            (
                nameRequest,
                result => { },
                error  => { }
            );
        }
    }

    public void Load(Action<SaveData> dataCallback)
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            Load_(dataCallback);
        }
        else
        {
            Login(result => Load_(dataCallback));
        }
        
        void Load_(Action<SaveData> dataCallback_)
        {
            var request = new GetUserDataRequest
            {
                Keys = new List<string>()
            };
            PlayFabClientAPI.GetUserData
            (
                request,
                result =>
                {
                    Dictionary<string, string> strStrDict = result.Data
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);
                    dataCallback_.Invoke(SaveData.FromDictionary(strStrDict));
                },
                error  => {}
            );
        }
    }

    void Login(Action<LoginResult> resultCallback, Action<PlayFabError> errorCallback = null)
    {
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier
        };
        PlayFabClientAPI.LoginWithCustomID
        (
            request,
            resultCallback,
            errorCallback
        );
    }
}
