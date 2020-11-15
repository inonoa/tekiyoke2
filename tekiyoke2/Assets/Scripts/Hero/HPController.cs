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
    HashSet<string> mutekiFilters = new HashSet<string>();
    public void AddMutekiFilter(string key)
    {
        mutekiFilters.Add(key);
    }
    public void RemoveMutekiFilter(string key)
    {
        mutekiFilters.Remove(key);
    }
    public bool CanBeDamaged => mutekiFilters.Count == 0;

    [Space(10), SerializeField] HeroMover hero;
    [SerializeField] IHPView view;

    public int HP{ get; private set; } = 3;

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

    void OnDamaged(int oldHP, int newHP)
    {
        const string DMG = "Damage";
        AddMutekiFilter(DMG);
        DelayedCall(hero.Parameters.MutekiSeconds, () => RemoveMutekiFilter(DMG));

        view.OnDamaged(HP, HP - 1); // u-n
    }

    void DelayedCall(float delay, DG.Tweening.TweenCallback call)
    {
        DOVirtual.DelayedCall(delay, call).AsHeros().GetPausable().AddTo(this);
    }
}

public interface IHPView
{
    void OnDamaged(int oldHP, int newHP);
    void OnHealed (int oldHP, int newHP);
}
