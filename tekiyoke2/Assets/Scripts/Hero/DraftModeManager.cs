using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Draft;
using DG.Tweening;

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
        .SetUpdate(true));
    }

    public void Exit()
    {
        KillAllTweens();

        InDraftMode = false;

        hero.HpCntr.RemoveMutekiFilter("DraftMode");

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
        .OnComplete(() => draftPEffect.SetActive(false)));
    }

    void KillAllTweens()
    {
        currentTweens.ForEach(tw => tw.Kill());
        currentTweens.Clear();
    }

    void Start()
    {
        draftPEffect = CameraController.CurrentCamera.AfterEffects.Find("Draft");

        windsManager.Init(() => new HeroInfo
        {
            pos      = hero.Transform.position,
            velocity = hero.velocity.ToVector2()
        });
    }
}
