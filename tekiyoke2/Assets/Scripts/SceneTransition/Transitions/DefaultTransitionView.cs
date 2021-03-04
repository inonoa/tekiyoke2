
using System;
using UniRx;

[Serializable]
public class DefaultTransitionView : ISceneTransitionView
{
    public IObservable<Unit> OnTransitionStart(SceneTransition sceneTransition)
    {
        return Observable.ReturnUnit();
    }

    public void OnNextSceneStart(SceneTransition sceneTransition)
    {
        PostEffectWrapper noise = CameraController.Current?.AfterEffects?.Find("Noise");
        noise?.SetVolume(1);
    }
}