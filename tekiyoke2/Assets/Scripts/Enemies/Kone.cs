using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Kone : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [field: SerializeField, LabelText(nameof(DPCD))]
    public DPinEnemy DPCD { get; private set; }

    [SerializeField] Collider2D heroSensor;

    new Transform transform;
    void Awake()
    {
        transform = base.transform;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        heroSensor.OnTriggerEnter2DAsObservable()
            .Where(other => other.CompareTag(TagNames.Hero))
            .Take(1)
            .Subscribe(_ => OnFindHero(HeroDefiner.CurrentPos))
            .AddTo(this);
    }

    void OnFindHero(Vector2 heroPos)
    {
        float jump = 350;
        
        Vector2 thisToHero = heroPos - (transform.position.ToVec2() + new Vector2(0, jump));
        transform.rotation = Quaternion.FromToRotation(Vector3.down, thisToHero);

        transform
            .DOMoveY(jump, 1f)
            .SetRelative()
            .SetEase(Ease.OutQuint)
            .onComplete += () => Attack(heroPos);
    }

    void Attack(Vector2 heroPos)
    {
        Vector2 thisToHero = heroPos - transform.position.ToVec2();
        Vector2 move = thisToHero.normalized * 700;
        transform.DOMove(move, 0.6f).SetRelative().SetEase(Ease.InOutSine)
            .onComplete += () =>
        {
            DOVirtual.DelayedCall(0.3f, () => ReJump(HeroDefiner.CurrentPos));
        };
    }

    void ReJump(Vector2 heroPos)
    {
        float jump = 200;
        float duration = 0.8f;
        
        Vector2 thisToHero = heroPos - (transform.position.ToVec2() + new Vector2(0, jump));
        float targetAngle = Quaternion.FromToRotation(Vector3.down, thisToHero).eulerAngles.z;

        transform.DOMyRotate(targetAngle, duration, clockwise: true);
        
        transform.DOMoveY(jump, duration).SetRelative().SetEase(Ease.InOutSine)
            .onComplete += () => ReAttack(heroPos);
    }

    void ReAttack(Vector2 heroPos)
    {
        Vector2 direction = (heroPos - transform.position.ToVec2()).normalized;
        float speed = 1000;
        float duration = 2f;
        
        transform.DOMove(direction * speed * duration, duration)
            .SetRelative()
            .SetEase(Ease.InSine)
            .onComplete += () => Destroy(gameObject);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}