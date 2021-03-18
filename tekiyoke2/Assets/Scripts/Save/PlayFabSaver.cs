using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PlayFab Saver", menuName = "Scriptable Object/PlayFabSaver")]
public class PlayFabSaver : SerializedScriptableObject, IDataSaver
{
    [SerializeField] PlayFabLoginManager login;
    
    public void Save(SaveData data)
    {
        if (login.IsLoggedIn())
        {
            Save_(data);
        }
        else
        {
            login.Login(() => Save_(data), error => Debug.Log(error.GenerateErrorReport()));
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

            // 応急処置？
            DOVirtual.DelayedCall(1f, () =>
            {
                var nameRequest = new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = data_.playerName
                };
                PlayFabClientAPI.UpdateUserTitleDisplayName
                (
                    nameRequest,
                    result => { },
                    error => { }
                );
            });
        }
    }

    public void Load(Action<SaveData> dataCallback)
    {
        if (login.IsLoggedIn())
        {
            Load_(dataCallback);
        }
        else
        {
            login.Login(() => Load_(dataCallback), error => Debug.Log(error.GenerateErrorReport()));
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
                    if (result.Data.Count == 0)
                    {
                        dataCallback_.Invoke(null);
                    }
                    else
                    {
                        Dictionary<string, string> strStrDict = result.Data
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);
                        dataCallback_.Invoke(SaveData.FromDictionary(strStrDict));
                    }
                },
                error  => {}
            );
        }
    }
}
