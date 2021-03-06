using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ShuttleView : MonoBehaviour, IShuttleView
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite sprite1L;
    [SerializeField] Sprite sprite2L;
    [SerializeField] Sprite sprite3L;
    [SerializeField] Sprite sprite1R;
    [SerializeField] Sprite sprite2R;
    [SerializeField] Sprite sprite3R;

    [SerializeField] new AudioSource audio;

    [Space(10)]
    [SerializeField] SpriteRenderer afterimagePrefab;
    [SerializeField] Transform worldTransform;
    [SerializeField] float afterimageChangeInterval = 0.1f;
    [SerializeField] float afterimageZ = 0;
    SpriteRenderer afterimage;

    [Space(10)]
    [SerializeField] SimpleAnim vanishEffectPrefab;

    bool goToRight;
    public void Init(EnemyShuttle shuttle)
    {
        this.goToRight = shuttle.GoToRight;
        spriteRenderer.sprite = goToRight ? sprite1R : sprite1L;
        
        afterimage = Instantiate(afterimagePrefab, worldTransform);
        SetAfterimage();
        
        Observable.Interval(TimeSpan.FromSeconds(afterimageChangeInterval))
            .Subscribe(_ => SetAfterimage())
            .AddTo(afterimage);
    }

    void SetAfterimage()
    {
        afterimage.transform.position = new Vector3
        (
            transform.position.x,
            transform.position.y,
            afterimageZ
        );
        afterimage.sprite = spriteRenderer.sprite;
    }
    
    public float OnHeroDetected()
    {
        DOVirtual.DelayedCall(0.1f, () => spriteRenderer.sprite = goToRight ? sprite2R : sprite2L, false)
            .GetPausable().AddTo(this);
        DOVirtual.DelayedCall(0.2f,  () => spriteRenderer.sprite = goToRight ? sprite3R : sprite3L, false)
            .GetPausable().AddTo(this);
        audio.Play();
        
        return 0.25f;
    }

    public float Vanish()
    {
        Destroy(afterimage.gameObject);
        spriteRenderer.enabled = false;
        
        var effect = Instantiate(vanishEffectPrefab, worldTransform);
        if(goToRight) effect.transform.localScale = new Vector3(-1, 1, 1);
        effect.transform.position = this.transform.position + new Vector3(goToRight ? -40 : 40, 0, -1);
        effect.Play(() => Destroy(effect.gameObject));
        
        return 0.4f;
    }
}