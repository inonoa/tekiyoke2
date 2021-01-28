using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Config
{
    public class ConfigView : MonoBehaviour, IConfigView
    {
        [SerializeField] Slider SESlider;
        [SerializeField] Slider BGMSlider;
        [FormerlySerializedAs("ExitButton")] [SerializeField] Button exitButton;
        [SerializeField] InputField nameField;
        [SerializeField] FocusNode nameNode;

        [SerializeField] UIFocusManager focusManager;
        [SerializeField] Image cursor;
        [SerializeField] float cursorRotateSpeed = 1;
        [SerializeField] SoundGroup sounds;
        
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

            this.UpdateAsObservable().Subscribe(_ =>
            {
                cursor.transform.Rotate(0, 0, cursorRotateSpeed * Time.deltaTime);
            });
        }

        void Start()
        {
            focusManager.OnNodeFocused.Skip(1).Subscribe(node =>
            {
                cursor.transform.position =
                    node.GetComponent<CursorPositionHolder>().CursorPosTransform.position;
                sounds.Play("Move");
            });

            nameNode.OnSelected.Subscribe(_ => nameField.ActivateInputField());

            exitButton.GetComponent<FocusNode>().OnSelected.Subscribe(_ => Exit());
        }

        void Exit()
        {
            sounds.Play("Enter");
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