using System;
using PlayFab;

public interface IPlayFabLogin
{
    void Login(Action onSuccess, Action<PlayFabError> onError);
}
