
using System;
using System.Linq;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GenerateIDLogin : IPlayFabLogin
{
    const string Key = "PlayFabLoginID";
    
    public void Login(Action onSuccess, Action<PlayFabError> onError)
    {
        var idIfExisting = PlayerPrefs.GetString(Key);

        if (string.IsNullOrEmpty(idIfExisting))
        {
            CreateAccountLogin(onSuccess, onError);
        }
        else
        {
            LoginWithExistingID(idIfExisting, onSuccess, onError);
        }
    }

    void CreateAccountLogin(Action onSuccess, Action<PlayFabError> onError)
    {
        var req = new LoginWithCustomIDRequest()
        {
            CustomId = GenerateRandomString(),
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID
        (
            req,
            result =>
            {
                if (!result.NewlyCreated)
                {
                    CreateAccountLogin(onSuccess, onError);
                    return;
                }
                PlayerPrefs.SetString(Key, req.CustomId);
                onSuccess?.Invoke();
            },
            onError
        );
    }

    void LoginWithExistingID(string id, Action onSuccess, Action<PlayFabError> onError)
    {
        var req = new LoginWithCustomIDRequest()
        {
            CustomId = id,
            CreateAccount = false
        };
        PlayFabClientAPI.LoginWithCustomID
        (
            req,
            _ => onSuccess?.Invoke(),
            onError
        );
    }

    static string GenerateRandomString()
    {
        const int length = 32;
        const string characters = "0123456789abcdefghijklmnopqrstuvwxyz";
        
        StringBuilder builder = new StringBuilder();
        foreach (var _ in Enumerable.Range(0, length))
        {
            builder.Append(characters[Random.Range(0, characters.Length)]);
        }
        
        return builder.ToString();
    }
}
