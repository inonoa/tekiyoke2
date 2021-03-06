
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
    public void Login(Action onSuccess, Action<PlayFabError> onError)
    {
        string id = LoadOrGenerateID();
        
        PlayFabClientAPI.LoginWithCustomID
        (
            new LoginWithCustomIDRequest()
            {
                CustomId = id,
                CreateAccount = true
            },
            _ => onSuccess?.Invoke(),
            onError
        );
    }
    
    const string Key = "PlayFabLoginID";
    string LoadOrGenerateID()
    {
        var idIfExisting = PlayerPrefs.GetString(Key);

        if (string.IsNullOrEmpty(idIfExisting))
        {
            string generated = GenerateRandomString();
            Debug.Log(generated);
            PlayerPrefs.SetString(Key, generated);
            return generated;
        }
        else
        {
            return idIfExisting;
        }
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
