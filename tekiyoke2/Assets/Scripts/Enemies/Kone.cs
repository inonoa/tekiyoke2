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

    [SerializeField] Collider2D groundSensor;

    [SerializeField] KoneTsuchi tsuchiPrefab;

    new Transform transform;
    Rigidbody2D rigidBody;
    void Awake()
    {
        transform = base.transform;
        rigidBody = GetComponent<Rigidbody2D>();
        
        groundSensor.OnTriggerEnter2DAsObservable() // 潜るときのつもりが地中から出た時も走ってる、まあこれはこれでいいか…………
            .Where(other => other.CompareTag("Terrain"))
            .Subscribe(_ => Instantiate
            (
                tsuchiPrefab,
                groundSensor.transform.position,
                Quaternion.identity,
                DraftManager.CurrentInstance.GameMasterTF
            )
            .Init(toRight: transform.rotation.eulerAngles.z.In(0, 180)) // toRight判定がずさん
            );
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        
        heroSensor.OnTriggerEnter2DAsObservable()
            .Where(other => other.CompareTag(TagNames.Hero))
            .Take(1)
            .Subscribe(_ => Jump(HeroDefiner.CurrentPos))
            .AddTo(this);
    }

    void Jump(Vector2 heroPos)
    {
        float jump = 350;
        
        Vector2 thisToHero = heroPos - (transform.position.ToVec2() + new Vector2(0, jump));
        transform.rotation = Quaternion.FromToRotation(Vector3.down, thisToHero);

        rigidBody
            .DOMoveY(jump, 0.5f)
            .SetRelative()
            .SetEase(Ease.OutQuint)
            .onComplete += () =>
        {
            DOVirtual.DelayedCall(0.1f, () => Attack(heroPos));
        };
    }

    void Attack(Vector2 heroPos)
    {
        Vector2 thisToHero = heroPos - transform.position.ToVec2();
        Vector2 move = thisToHero.normalized * 900;
        rigidBody.DOMove(move, 0.6f).SetRelative().SetEase(Ease.InOutSine)
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

        rigidBody.DOMyRotate(targetAngle, duration, clockwise: true);

        rigidBody.DOMoveY(jump, duration).SetRelative().SetEase(Ease.InOutSine)
            .onComplete += () => ReAttack(heroPos);
    }

    void ReAttack(Vector2 heroPos)
    {
        Vector2 direction = (heroPos - transform.position.ToVec2()).normalized;
        float speed = 1300;
        float duration = 3f;
        
        rigidBody.DOMove(direction * speed * duration, duration)
            .SetRelative()
            .SetEase(Ease.InQuad)
            .onComplete += () => Destroy(gameObject);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}