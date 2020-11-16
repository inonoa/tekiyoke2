using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using System.Linq;
using DG.Tweening;
using UniRx;

public class HPController : SerializedMonoBehaviour
{
    
    [SerializeField] HeroMover         hero;
    [SerializeField] IHPView           view;
    [SerializeField] HeroMutekiManager mutekiManager;
    public bool CanBeDamaged => mutekiManager.CanBeDamaged;

    public int HP { get; private set; } = 3;

    // 普通に(?)DamageとかHealとかで分ければよかった
    public void ChangeHP(int value)
    {
        if(value <= 0 && HP <= 0) return;
        if(value >= 3 && HP >= 3) return;

        if(value < HP)
        {
            if(value <= 0)      OnDamaged(HP, 0);
            else if(value == 1) OnDamaged(HP, 1);
            else if(value == 2) OnDamaged(HP, 2);
        }
        else if(value > HP)
        {
            if     (value == 3) view.OnHealed(HP, 3);
            else if(value == 2) view.OnHealed(HP, 2);
        }

        HP = value;
    }

    Tween removeMuteki;
    void OnDamaged(int oldHP, int newHP)
    {
        removeMuteki?.Kill();
        const string DMG = "Damage";
        mutekiManager.AddMutekiFilter(DMG);
        removeMuteki = DelayedCall(hero.Parameters.MutekiSeconds, () => mutekiManager.RemoveMutekiFilter(DMG));

        view.OnDamaged(HP, HP - 1); // u-n
    }

    Tween DelayedCall(float delay, DG.Tweening.TweenCallback call)
    {
        Tween tw = DOVirtual.DelayedCall(delay, call).AsHeros();
        tw.GetPausable().AddTo(this);
        return tw;
    }
}

public interface IHPView
{
    void OnDamaged(int oldHP, int newHP);
    void OnHealed (int oldHP, int newHP);
}
