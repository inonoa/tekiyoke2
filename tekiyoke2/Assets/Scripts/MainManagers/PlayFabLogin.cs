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
                result => {},
                error  => Debug.LogError(error.GenerateErrorReport())
            );
        }
    }
}