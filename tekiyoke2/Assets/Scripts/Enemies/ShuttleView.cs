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

    [SerializeField] SpriteRenderer afterimagePrefab;
    [SerializeField] Transform worldTransform;
    [SerializeField] float afterimageChangeInterval = 0.1f;
    [SerializeField] float afterimageZ = 0;
    SpriteRenderer afterimage;

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
        
        return 0.25f;
    }

    public float Vanish()
    {
        Destroy(afterimage.gameObject);
        return 0.4f;
    }
}