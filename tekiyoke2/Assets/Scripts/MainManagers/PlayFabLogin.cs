using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace MainManagers
{
    public class PlayFabLogin
    {
        [RuntimeInitializeOnLoadMethod]
        static void Login()
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier //tmp
            };
            PlayFabClientAPI.LoginWithCustomID
            (
                request,
                result =>
                {
                    // SendName("inonoa");
                },
                HandleError
            );
        }

        static void SendName(string name)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = name
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName
            (
                request,
                result => { },
                HandleError
            );
        }

        static void HandleError(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }
    }
}