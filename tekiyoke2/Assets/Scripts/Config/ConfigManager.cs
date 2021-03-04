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
        [SerializeField] IPlayerNameChanger nameChanger;
        [SerializeField] SaveDataManager saveDataManager;

        void Start()
        {
            view.OnSEVolumeChanged.Subscribe(soundVolumeChanger.ChangeSEVolume).AddTo(this);
            view.OnBGMVolumeChanged.Subscribe(soundVolumeChanger.ChangeBGMVolume).AddTo(this);
            view.OnExit.Subscribe(_ =>
            {
                saveDataManager.SetSEVolume(soundVolumeChanger.SEVolume);
                saveDataManager.SetBGMVolume(soundVolumeChanger.BGMVolume);
                _OnExit.OnNext(Unit.Default);
            })
            .AddTo(this);
            view.OnPlayerNameChanged.Subscribe(nameChanger.ChangePlayerName).AddTo(this);
        }

        public void Enter()
        {
            view.Enter(soundVolumeChanger.BGMVolume, soundVolumeChanger.SEVolume, nameChanger.PlayerName);
        }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;
    }

    public interface IConfigView
    {
        void Enter(float bgmVolume, float seVolume, string playerName);
        IObservable<float> OnBGMVolumeChanged { get; }
        IObservable<float> OnSEVolumeChanged  { get; }
        IObservable<Unit> OnExit { get; }
        IObservable<string> OnPlayerNameChanged { get; }
    }

    public interface ISoundVolumeChanger
    {
        void ChangeSEVolume(float volume);
        void ChangeBGMVolume(float volume);
        float SEVolume { get; }
        float BGMVolume { get; }
    }

    public interface IPlayerNameChanger
    {
        void ChangePlayerName(string name);
        string PlayerName { get; }
    }
}