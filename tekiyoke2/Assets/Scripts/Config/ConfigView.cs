using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Config
{
    public class ConfigView : MonoBehaviour, IConfigView
    {
        [SerializeField] Slider     SESlider;
        [SerializeField] Slider     BGMSlider;
        [FormerlySerializedAs("ExitButton")] [SerializeField] Button     exitButton;
        [SerializeField] InputField nameField;
        
        public void Enter(float bgmVolume, float seVolume, string playerName)
        {
            gameObject.SetActive(true);
            SESlider.value  = seVolume;
            BGMSlider.value = bgmVolume;
            nameField.text  = playerName;
        }

        void Awake()
        {
            OnBGMVolumeChanged = BGMSlider.OnValueChangedAsObservable();
            OnSEVolumeChanged  = SESlider.OnValueChangedAsObservable();
            OnPlayerNameChanged = nameField.OnValueChangedAsObservable();
            exitButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                _OnExit.OnNext(Unit.Default);
            });
        }

        public IObservable<float> OnBGMVolumeChanged { get; private set; }
        public IObservable<float> OnSEVolumeChanged { get; private set; }
        public IObservable<string> OnPlayerNameChanged { get; private set; }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;
    }
}