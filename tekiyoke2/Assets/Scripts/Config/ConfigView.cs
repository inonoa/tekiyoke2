using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Config
{
    public class ConfigView : MonoBehaviour, IConfigView
    {
        [SerializeField] Slider SESlider;
        [SerializeField] Slider BGMSlider;
        [SerializeField] Button ExitButton;
        
        public void Enter(float bgmVolume, float seVolume)
        {
            gameObject.SetActive(true);
            SESlider.value  = seVolume;
            BGMSlider.value = bgmVolume;
        }

        void Awake()
        {
            OnBGMVolumeChanged = BGMSlider.OnValueChangedAsObservable();
            OnSEVolumeChanged  = SESlider.OnValueChangedAsObservable();
            ExitButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                _OnExit.OnNext(Unit.Default);
            });
        }

        public IObservable<float> OnBGMVolumeChanged { get; private set; }
        public IObservable<float> OnSEVolumeChanged { get; private set; }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;
    }
}