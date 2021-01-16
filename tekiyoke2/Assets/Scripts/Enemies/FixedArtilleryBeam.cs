using System;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class FixedArtilleryBeam : MonoBehaviour
{
    [SerializeField] float readySeconds = 1f;
    [SerializeField] float readyBeamVisibleSeconds = 0.4f;
    [SerializeField] float shootSeconds = 1f;
    [SerializeField] float mainBeamVisibleSeconds = 0.5f;
    [SerializeField] float beamLengthMax = 1000;
    [SerializeField] new LineRenderer renderer;
    [SerializeField] new EdgeCollider2D collider;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] Transform body;

    [SerializeField] Texture2D readyTex;
    [SerializeField] Texture2D mainTex;

    Vector2? lastHitPos;

    public IObservable<Unit> BeReady() => BeReady(transform.position);
    
    IObservable<Unit> BeReady(Vector3 origin)
    {
        print("ready");
        Vector2 originToHero = HeroDefiner.CurrentPos - origin;
        var hit = Physics2D.Raycast(origin + originToHero.normalized.ToVec3(), originToHero, beamLengthMax, LayerMask.GetMask("Terrain"));
        if (hit.collider == null) lastHitPos = null;
        else lastHitPos = hit.point - originToHero.normalized * 3;
        Vector3 endPos = hit.collider != null ? hit.point.ToVec3() : (origin + originToHero.normalized.ToVec3() * beamLengthMax);
        
        renderer.material.SetTexture("_MainTex", readyTex);
        renderer.enabled = true;
        collider.enabled = false;
        
        SetPositions(origin, endPos);

        DOVirtual.DelayedCall(readyBeamVisibleSeconds, () => renderer.enabled = false, false).GetPausable().AddTo(this);
        
        var end = new Subject<Unit>();
        DOVirtual.DelayedCall(readySeconds, () => end.OnNext(Unit.Default), false).GetPausable().AddTo(this);

        return end;
    }

    public IObservable<Unit> StartShoot()
    {
        Subject<Unit> allEnded = new Subject<Unit>();

        Shoot()
        .Subscribe(_ =>
        {
            if (! lastHitPos.HasValue)
            {
                allEnded.OnNext(Unit.Default);
                print("いっかいでおわり");
                return;
            }

            BeReady(lastHitPos.Value.ToVec3())
            .Subscribe(__ =>
            {
                Shoot().Subscribe(___ => allEnded.OnNext(Unit.Default));
            });
        });

        return allEnded;
    }
    
    IObservable<Unit> Shoot()
    {
        collider.enabled = true;
        rigidBody.WakeUp();
        renderer.enabled = true;
        renderer.material.SetTexture("_MainTex", mainTex);

        DOVirtual.DelayedCall(mainBeamVisibleSeconds, () =>
        {
            collider.enabled = false;
            renderer.enabled = false;
        },
            false).GetPausable().AddTo(this);

        var end = new Subject<Unit>();
        DOVirtual.DelayedCall(shootSeconds, () =>
        {
            end.OnNext(Unit.Default);
        },
            false).GetPausable().AddTo(this);

        return end;
    }

    void SetPositions(Vector3 start, Vector3 end)
    {
        renderer.SetPositions(new []{start, end});
        collider.points = new[]
        {
            (start - collider.transform.position).ToVec2(),
            (end   - collider.transform.position).ToVec2()
        };
        if (body.localScale.x < 0)
        {
            collider.points = collider.points.Select(MyMath.FlipX).ToArray();
        }
    }
}