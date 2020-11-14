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
    [SerializeField] HPSprites sprites;
    [SerializeField] Image image;

    [SerializeField] float mutekiSecondsAfterDamage = 1.6f;


    [SerializeField, ReadOnly] HashSet<string> mutekiFilters = new HashSet<string>();
    public void AddMutekiFilter(string key)
    {
        mutekiFilters.Add(key);
    }
    public void RemoveMutekiFilter(string key)
    {
        mutekiFilters.Remove(key);
    }
    public bool CanBeDamaged => mutekiFilters.Count == 0;


    new CameraController camera;
    [SerializeField] Vector3 cameraShakeWidth   = new Vector3(30, 30, 0);
    [SerializeField] float cameraShakeSeconds = 0.2f;
    [SerializeField] int   cameraShakeVibrato = 10;
    [SerializeField] float houtaiRedSeconds  = 0.3f;
    [SerializeField] float houtaiBlueSeconds = 0.7f;
    [SerializeField] float houtaiAlpha1Seconds = 1f;

    public int HP{ get; private set; } = 3;

    ///<summary>HPの増減はすべてここから。</summary>
    public void ChangeHP(int value)
    {
        if(value <= 0 && HP <= 0) return;
        if(value >= 3 && HP >= 3) return;

        if(value < HP)
        {
            if(value <= 0)      OnDamaged(sprites.Img1_0, sprites.Img0);
            else if(value == 1) OnDamaged(sprites.Img2_1, sprites.Img1);
            else if(value == 2) OnDamaged(sprites.Img3_2, sprites.Img2);
        }
        else if(value > HP)
        {
            if     (value == 3) OnHealed(sprites.Img2_3, sprites.Img3);
            else if(value == 2) OnHealed(sprites.Img1_2, sprites.Img2);
        }

        HP = value;
    }

    void OnDamaged(Sprite spriteBeingDamaged, Sprite spriteAfterDamage)
    {
        const string DMG = "Damage";
        AddMutekiFilter(DMG);
        DelayedCall(mutekiSecondsAfterDamage, () => RemoveMutekiFilter(DMG));

        camera.transform.DOShakePosition(cameraShakeSeconds, cameraShakeWidth, cameraShakeVibrato);

        image.color  = Color.white;
        image.sprite = spriteBeingDamaged;

        DelayedCall(houtaiRedSeconds,    () => image.sprite = spriteAfterDamage);
        DelayedCall(houtaiAlpha1Seconds, () => image.color  = new Color(1, 1, 1, 0.7f));
    }

    void OnHealed(Sprite spriteBeingHealed, Sprite spriteAfterHeal)
    {
        image.color  = Color.white;
        image.sprite = spriteBeingHealed;

        DelayedCall(houtaiBlueSeconds,   () => image.sprite = spriteAfterHeal);
        DelayedCall(houtaiAlpha1Seconds, () => image.color  = new Color(1, 1, 1, 0.7f));
    }

    void DelayedCall(float delay, DG.Tweening.TweenCallback call)
    {
        DOVirtual.DelayedCall(delay, call).AsHeros().GetPausable().AddTo(this);
    }

    void Start()
    {
        camera = CameraController.CurrentCamera;
    }
}

[Serializable]
public class HPSprites
{
    [field: SerializeField, LabelText("Image 3")] public Sprite Img3{ get; private set; }
    [field: SerializeField, LabelText("Image 2")] public Sprite Img2{ get; private set; }
    [field: SerializeField, LabelText("Image 1")] public Sprite Img1{ get; private set; }
    [field: SerializeField, LabelText("Image 0")] public Sprite Img0{ get; private set; }

    [field: Space(10)]
    [field: SerializeField, LabelText("Image 3 -> 2")] public Sprite Img3_2{ get; private set; }
    [field: SerializeField, LabelText("Image 2 -> 1")] public Sprite Img2_1{ get; private set; }
    [field: SerializeField, LabelText("Image 1 -> 0")] public Sprite Img1_0{ get; private set; }

    [field: Space(10)]
    [field: SerializeField, LabelText("Image 2 -> 3")] public Sprite Img2_3{ get; private set; }
    [field: SerializeField, LabelText("Image 1 -> 2")] public Sprite Img1_2{ get; private set; }
}
