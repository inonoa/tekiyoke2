using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Draft;
using DG.Tweening;
using System.Linq;
using UniRx;

public class DraftModeManager : MonoBehaviour
{
    public bool InDraftMode{ get; private set; } = false;

    [SerializeField] float enterDuration = 1f;
    [SerializeField] float exitDuration  = 1f;

    [SerializeField] DraftWindsManager windsManager;
    PostEffectWrapper draftPEffect;
    [SerializeField] HeroMover hero;
    [SerializeField] DPManager dpManager;

    List<Tween> currentTweens = new List<Tween>();

    const string MutekiKey = "DraftMode";

    public void TryEnter()
    {
        if(InDraftMode) return;

        if(dpManager.DP <= 0)
        {
            //なんかする？
            return;
        }

        Enter();
    }

    void Enter()
    {
        KillAllTweens();

        InDraftMode = true;
        
        hero.TimeManager.SetTimeScaleExceptHero(hero.DraftModeParams.TimeScale);
        
        if(hero.DraftModeParams.Muteki) hero.MutekiManager.AddMutekiFilter(MutekiKey);

        windsManager.SetActive(true);

        draftPEffect.SetActive(true);
        draftPEffect.SetVolume(-0.1f);
        currentTweens.Add(DOTween.To
        (
            draftPEffect.GetVolume,
            draftPEffect.SetVolume,
            1.1f,
            enterDuration
        )
        .SetUpdate(true)
        .OnComplete(() => currentTweens.Clear()));
    }

    public void TryExit()
    {
        if(!InDraftMode) return;

        Exit();
    }

    void Exit()
    {
        KillAllTweens();

        InDraftMode = false;

        hero.TimeManager.SetTimeScaleExceptHero(1);
        hero.MutekiManager.RemoveMutekiFilter(MutekiKey);

        currentTweens.Add(DOVirtual.DelayedCall(
            exitDuration / 2,
            () => windsManager.SetActive(false)
        )
        .SetUpdate(true));

        currentTweens.Add(DOTween.To
        (
            draftPEffect.GetVolume,
            draftPEffect.SetVolume,
            -0.1f,
            exitDuration
        )
        .SetUpdate(true)
        .OnComplete(() =>
        {
            draftPEffect.SetActive(false);
            currentTweens.Clear();
        }));
    }

    void KillAllTweens()
    {
        currentTweens.ForEach(tw => tw.Kill());
        currentTweens.Clear();
    }

    void OnPause()
    {
        currentTweens.ForEach(tw => tw.Pause());
    }

    void OnPauseEnd()
    {
        currentTweens.ForEach(tw => tw.TogglePause());
    }

    void Start()
    {
        draftPEffect = CameraController.CurrentCamera.AfterEffects.Find("Draft");

        windsManager.Init
        (
            () => new HeroInfo
            {
                pos      = hero.Transform.position,
                velocity = hero.velocity.ToVector2()
            },
            () => hero.TimeManager.DeltaTimeAroundHero,
            () => hero.TimeManager.TimeAroundHero
        );

        Pauser.Instance.OnPause
            .Subscribe(_ => OnPause());
        Pauser.Instance.OnPauseEnd
            .Subscribe(_ => OnPauseEnd());
    }

    void Update()
    {
        if(InDraftMode)
        {
            float dp = hero.DraftModeParams.DpPerSecond * TimeManager.Current.DeltaTimeAroundHero;
            bool enoughDP = dpManager.ForceUseDP(dp);

            if(!enoughDP)
            {
                Exit();
            }
        }
    }
}
