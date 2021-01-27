using System;
using DG.Tweening;
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

            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            transform.SetLocalX(100);
            canvasGroup.DOFade(1, 0.4f).SetEase(Ease.OutCubic);
            transform.DOLocalMoveX(0, 0.4f);
        }

        void Awake()
        {
            OnBGMVolumeChanged = BGMSlider.OnValueChangedAsObservable();
            OnSEVolumeChanged  = SESlider.OnValueChangedAsObservable();
            OnPlayerNameChanged = nameField.OnValueChangedAsObservable();
            exitButton.onClick.AddListener(Exit);
        }

        void Exit()
        {
            GetComponent<CanvasGroup>().DOFade(0, 0.4f).SetEase(Ease.OutCubic);
            transform.DOLocalMoveX(-100, 0.4f);
            DOVirtual.DelayedCall(0.4f, () => gameObject.SetActive(false));
            DOVirtual.DelayedCall(0.2f, () => _OnExit.OnNext(Unit.Default));
        }

        public IObservable<float> OnBGMVolumeChanged { get; private set; }
        public IObservable<float> OnSEVolumeChanged { get; private set; }
        public IObservable<string> OnPlayerNameChanged { get; private set; }
        
        Subject<Unit> _OnExit = new Subject<Unit>();
        public IObservable<Unit> OnExit => _OnExit;
    }
}