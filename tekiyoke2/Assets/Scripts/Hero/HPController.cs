using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using System.Linq;
using DG.Tweening;
using UniRx;

public class HPController : MonoBehaviour
{
    const string DEP = "依存";
    [FoldoutGroup(DEP), SerializeField] Sprite img3;
    [FoldoutGroup(DEP), SerializeField] Sprite img2;
    [FoldoutGroup(DEP), SerializeField] Sprite img1;
    [FoldoutGroup(DEP), SerializeField] Sprite img0;

    [Space(10)]
    [FoldoutGroup(DEP), SerializeField] Sprite img3_2;
    [FoldoutGroup(DEP), SerializeField] Sprite img2_1;
    [FoldoutGroup(DEP), SerializeField] Sprite img1_0;

    [Space(10)]
    [FoldoutGroup(DEP), SerializeField] Sprite img2_3;
    [FoldoutGroup(DEP), SerializeField] Sprite img1_2;

    [Space(10)]
    [FoldoutGroup(DEP), SerializeField] Image image;

    [SerializeField] float mutekiSecondsAfterDamage = 1.6f;


    [SerializeField, ReadOnly] string[] mutekiFilters = new string[0];
    public bool CanBeDamaged => mutekiFilters.Length == 0;
    public void AddMutekiFilter(string key)
    {
        if(mutekiFilters.Contains(key)) return;
        mutekiFilters = mutekiFilters.Concat(new string[1]{ key }).ToArray();
    }
    public void RemoveMutekiFilter(string key)
    {
        mutekiFilters = mutekiFilters.Where(k => k != key).ToArray();
    }
    

    ///<summary>ここを直接書き換えない</summary>
    int hp = 3;

    new CameraController camera;
    [SerializeField] Vector3 cameraShakeWidth   = new Vector3(30, 30, 0);
    [SerializeField] float cameraShakeSeconds = 0.2f;
    [SerializeField] int   cameraShakeVibrato = 10;
    [SerializeField] float houtaiRedSeconds  = 0.3f;
    [SerializeField] float houtaiBlueSeconds = 0.7f;
    [SerializeField] float houtaiAlpha1Seconds = 1f;


    ///<summary>HPの増減はすべてここから。</summary>
    public void ChangeHP(int value)
    {
        if(value <= 0 && HP <= 0) return;
        if(value >= 3 && HP >= 3) return;

        if(value < HP)
        {
            if(value <= 0)      OnDamaged(img1_0, img0);
            else if(value == 1) OnDamaged(img2_1, img1);
            else if(value == 2) OnDamaged(img3_2, img2);
        }
        else if(value > HP)
        {
            if     (value == 3) OnHealed(img2_3, img3);
            else if(value == 2) OnHealed(img1_2, img2);
        }

        hp = value;
    }

    void OnDamaged(Sprite spriteBeingDamaged, Sprite spriteAfterDamage)
    {
        const string DMG = "Damage";
        AddMutekiFilter(DMG);
        DOVirtual.DelayedCall(mutekiSecondsAfterDamage, () => RemoveMutekiFilter(DMG))
            .GetPausable()
            .AddTo(this);

        camera.transform.DOShakePosition(cameraShakeSeconds, cameraShakeWidth, cameraShakeVibrato);

        image.color  = Color.white;
        image.sprite = spriteBeingDamaged;

        DOVirtual.DelayedCall(houtaiRedSeconds,    () => image.sprite = spriteAfterDamage)
            .GetPausable()
            .AddTo(this);
        DOVirtual.DelayedCall(houtaiAlpha1Seconds, () => image.color  = new Color(1, 1, 1, 0.7f))
            .GetPausable()
            .AddTo(this);
    }

    void OnHealed(Sprite spriteBeingHealed, Sprite spriteAfterHeal)
    {
        image.color  = Color.white;
        image.sprite = spriteBeingHealed;

        DOVirtual.DelayedCall(houtaiBlueSeconds,   () => image.sprite = spriteAfterHeal)
            .GetPausable()
            .AddTo(this);
        DOVirtual.DelayedCall(houtaiAlpha1Seconds, () => image.color  = new Color(1, 1, 1, 0.7f))
            .GetPausable()
            .AddTo(this);
    }

    public int HP => hp;

    void Start()
    {
        camera = CameraController.CurrentCamera;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) ChangeHP(3);
    }
}
