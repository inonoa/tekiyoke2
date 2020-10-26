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

    List<Tween> currentTweens = new List<Tween>();

    public void Enter()
    {
        KillAllTweens();

        InDraftMode = true;

        hero.HpCntr.AddMutekiFilter("DraftMode");
        hero.TimeManager.SetTimeScaleExceptHero(0.2f);

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

    public void Exit()
    {
        KillAllTweens();

        InDraftMode = false;

        hero.HpCntr.RemoveMutekiFilter("DraftMode");
        hero.TimeManager.SetTimeScaleExceptHero(1);

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
}
