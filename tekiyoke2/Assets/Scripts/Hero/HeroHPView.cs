using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UniRx;

public class HeroHPView : MonoBehaviour, IHPView
{
    [SerializeField] Image image;
    [Space(10), SerializeField] float houtaiRedSeconds  = 0.3f;
    [SerializeField] float houtaiBlueSeconds = 0.7f;
    [SerializeField] float houtaiAlpha1Seconds = 1f;
    

    CameraController Camera => CameraController.CurrentCamera;
    [SerializeField] Vector3 cameraShakeWidth   = new Vector3(30, 30, 0);
    [SerializeField] float cameraShakeSeconds = 0.2f;
    [SerializeField] int   cameraShakeVibrato = 10;
    
    [SerializeField] HPSprites sprites;

    List<Tween> delayedSpriteChanges = new List<Tween>();

    public void OnDamaged(int oldHP, int newHP)
    {
        ClearTweens();
        
        Camera.transform.DOShakePosition(cameraShakeSeconds, cameraShakeWidth, cameraShakeVibrato);

        image.color  = Color.white;
        image.sprite = BeingDamagedSprite(newHP);

        delayedSpriteChanges.Add(DelayedCall
        (
            houtaiRedSeconds,
            () => image.sprite = NewSprite(newHP)
        ));
        delayedSpriteChanges.Add(DelayedCall
        (
            houtaiAlpha1Seconds,
            () => image.color  = new Color(1, 1, 1, 0.7f)
        ));
    }

    public void OnHealed(int oldHP, int newHP)
    {
        ClearTweens();
        
        image.color  = Color.white;
        image.sprite = BeingHealedSprite(newHP);

        delayedSpriteChanges.Add(DelayedCall
        (
            houtaiBlueSeconds,
            () => image.sprite = NewSprite(newHP)
        ));
        delayedSpriteChanges.Add(DelayedCall
        (
            houtaiAlpha1Seconds,
            () => image.color  = new Color(1, 1, 1, 0.7f)
        ));
    }

    Sprite BeingDamagedSprite(int newHP)
    {
        switch(newHP)
        {
            case 0: return sprites.Img1_0;
            case 1: return sprites.Img2_1;
            case 2: return sprites.Img3_2;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    Sprite BeingHealedSprite(int newHP)
    {
        switch(newHP)
        {
            case 2: return sprites.Img1_2;
            case 3: return sprites.Img2_3;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    Sprite NewSprite(int newHP)
    {
        switch(newHP)
        {
            case 0: return sprites.Img0;
            case 1: return sprites.Img1;
            case 2: return sprites.Img2;
            case 3: return sprites.Img3;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    void ClearTweens()
    {
        foreach (Tween spriteChange in delayedSpriteChanges)
        {
            spriteChange.Kill();
        }
        delayedSpriteChanges = new List<Tween>();
    }

    Tween DelayedCall(float delay, DG.Tweening.TweenCallback call)
    {
        Tween tw = DOVirtual.DelayedCall(delay, call).AsHeros();
        tw.GetPausable().AddTo(this);
        return tw;
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
