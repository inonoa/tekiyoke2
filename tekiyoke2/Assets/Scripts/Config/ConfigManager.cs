using System;
using Sirenix.OdinInspector;
using SoundOrMusic;
using UniRx;
using UnityEngine;

namespace Config
{
    public class ConfigManager : SerializedMonoBehaviour
    {
        [SerializeField] IConfigView view;
        [SerializeField] ISoundVolumeChanger soundVolumeChanger;

        void Start()
        {
            view.OnSEVolumeChanged.Subscribe(soundVolumeChanger.ChangeSEVolume);
            view.OnBGMVolumeChanged.Subscribe(soundVolumeChanger.ChangeBGMVolume);
            view.OnExit.Subscribe(_ => _OnExit.OnNext(Unit.Default));
        }

        public void Enter()
        {
            view.Enter(soundVolumeChanger.BGMVolume, soundVolumeChanger.SEVolume);
        }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;
    }

    public interface IConfigView
    {
        void Enter(float bgmVolume, float seVolume);
        IObservable<float> OnBGMVolumeChanged { get; }
        IObservable<float> OnSEVolumeChanged  { get; }
        IObservable<Unit> OnExit { get; }
    }

    public interface ISoundVolumeChanger
    {
        void ChangeSEVolume(float volume);
        void ChangeBGMVolume(float volume);
        float SEVolume { get; }
        float BGMVolume { get; }
    }
}