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

    [SerializeField] Collider2D mainCollider;

    [SerializeField] Collider2D heroSensor;

    [SerializeField] Collider2D groundSensor;

    [SerializeField] KoneTsuchi tsuchiPrefab;

    [SerializeField] SimpleAnim anim;

    [SerializeField] SpriteRenderer spriteRenderer;

    new Transform transform;
    Rigidbody2D rigidBody;
    void Awake()
    {
        transform = base.transform;
        rigidBody = GetComponent<Rigidbody2D>();
        
        groundSensor.OnTriggerEnter2DAsObservable() // 潜るときのつもりが地中から出た時も走ってる、まあこれはこれでいいか…………
            .Where(other => other.CompareTag(Tags.Terrain))
            .Subscribe(_ => Instantiate
            (
                tsuchiPrefab,
                groundSensor.transform.position,
                Quaternion.identity,
                DraftManager.CurrentInstance.GameMasterTF
            )
            .Init(toRight: transform.rotation.eulerAngles.z.In(0, 180))); // toRight判定がずさん
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        
        heroSensor.OnTriggerEnter2DAsObservable()
            .Where(other => other.CompareTag(Tags.Hero))
            .Take(1)
            .Subscribe(_ => Jump(HeroDefiner.CurrentPos))
            .AddTo(this);

        anim.Play();
    }

    [Space(10)]
    [SerializeField, FoldoutGroup("First Jump")] float firstJumpHeight    = 350;
    [SerializeField, FoldoutGroup("First Jump")] float firstJumpDuration  = 0.5f;
    [SerializeField, FoldoutGroup("First Jump")] Ease  firstJumpEase      = Ease.InQuint;
    [SerializeField, FoldoutGroup("First Jump")] float firstJumpToAttack  = 0.1f;
    [SerializeField, FoldoutGroup("First Jump")] float lookAtHeroDelay    = 0.7f;
    [SerializeField, FoldoutGroup("First Jump")] float lookAtHeroDuration = 0.2f;
    void Jump(Vector2 heroPos)
    {
        Vector2 targetPos = transform.position.ToVec2() + new Vector2(0, firstJumpHeight);
        Vector2 thisToHero = heroPos - targetPos;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, thisToHero);

        Vector2 nextHeroPos = heroPos;
        rigidBody
            .DOMoveY(firstJumpHeight, firstJumpDuration)
            .SetRelative()
            .SetEase(firstJumpEase)
            .onComplete += () =>
        {
            DOVirtual.DelayedCall(firstJumpToAttack, () => Attack(nextHeroPos));
        };

        DOVirtual.DelayedCall(lookAtHeroDelay, () =>
        {
            nextHeroPos = HeroDefiner.CurrentPos.ToVec2();
            Vector2 thisToHeroFinal = nextHeroPos - targetPos;
            float targetRot = Quaternion.FromToRotation(Vector3.down, thisToHeroFinal).eulerAngles.z;
            rigidBody.DOMyRotate(targetRot, lookAtHeroDuration, false).SetEase(Ease.InOutSine);
        });
    }

    [SerializeField, FoldoutGroup("First Attack")] float firstAttackMove         = 900;
    [SerializeField, FoldoutGroup("First Attack")] float firstAttackDuration     = 0.6f;
    [SerializeField, FoldoutGroup("First Attack")] Ease  firstAttackEase         = Ease.InOutSine;
    [SerializeField, FoldoutGroup("First Attack")] float firstAttackToSecondJump = 0.3f;
    void Attack(Vector2 heroPos)
    {
        Vector2 thisToHero = heroPos - transform.position.ToVec2();
        Vector2 move = thisToHero.normalized * firstAttackMove;
        
        rigidBody.DOMove(move, firstAttackDuration)
            .SetRelative()
            .SetEase(firstAttackEase)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(firstAttackToSecondJump, () => ReJump(HeroDefiner.CurrentPos));
            })
            .GetPausable()
            .AddTo(this);
    }

    [SerializeField, FoldoutGroup("Second Jump")] float secondJumpHeight         = 200;
    [SerializeField, FoldoutGroup("Second Jump")] float secondJumpDuration       = 1f;
    [SerializeField, FoldoutGroup("Second Jump")] Ease  secondJumpEase           = Ease.InOutSine;
    [SerializeField, FoldoutGroup("Second Jump")] float secondJumpToSecondAttack = 0;
    void ReJump(Vector2 heroPos)
    {
        Vector2 thisToHero = heroPos - (transform.position.ToVec2() + new Vector2(0, secondJumpHeight));
        float targetAngle = Quaternion.FromToRotation(Vector3.down, thisToHero).eulerAngles.z;

        rigidBody.DOMyRotate(targetAngle, secondJumpDuration, clockwise: true);

        rigidBody.DOMoveY(secondJumpHeight, secondJumpDuration).SetRelative().SetEase(secondJumpEase)
            .onComplete += () => ReAttack(heroPos);
    }

    [SerializeField, FoldoutGroup("Second Attack")] float secondAttackMove     = 3900;
    [SerializeField, FoldoutGroup("Second Attack")] float secondAttackDuration = 3f;
    [SerializeField, FoldoutGroup("Second Attack")] Ease  secondAttackEase     = Ease.InQuad;
    void ReAttack(Vector2 heroPos)
    {
        Vector2 direction = (heroPos - transform.position.ToVec2()).normalized;

        rigidBody.DOMove(direction * secondAttackMove, secondAttackDuration)
            .SetRelative()
            .SetEase(secondAttackEase)
            .GetPausable()
            .AddTo(this);

        const float fadeOutDur = 0.4f;
        DOVirtual.DelayedCall
        (
            secondAttackDuration - fadeOutDur,
            () =>
            {
                mainCollider.enabled = false;
                spriteRenderer.DOFade(0, fadeOutDur)
                    .OnComplete(() => Destroy(gameObject))
                    .GetPausable().AddTo(this);
            },
            false
        )
        .GetPausable()
        .AddTo(this);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
