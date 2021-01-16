using System;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class FixedArtilleryBeam : MonoBehaviour
{
    [SerializeField] float readySeconds = 1f;
    [SerializeField] float shootSeconds = 1f;
    [SerializeField] new LineRenderer renderer;
    [SerializeField] new EdgeCollider2D collider;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] Transform body;

    [SerializeField] Texture2D readyTex;
    [SerializeField] Texture2D mainTex;

    public IObservable<Unit> BeReady()
    {
        Vector3 thisPos = transform.position;
        Vector2 thisToHero = HeroDefiner.CurrentPos - thisPos;
        var hit = Physics2D.Raycast(thisPos, thisToHero, 1000, LayerMask.GetMask("Terrain"));
        Vector3 endPos = hit.collider != null ? hit.point.ToVec3() : (thisPos + thisToHero.normalized.ToVec3() * 1000);
        
        renderer.material.SetTexture("_MainTex", readyTex);
        renderer.enabled = true;
        collider.enabled = false;
        
        SetPositions(thisPos, endPos);

        DOVirtual.DelayedCall(0.4f, () => renderer.enabled = false, false).GetPausable().AddTo(this);
        
        var end = new Subject<Unit>();
        DOVirtual.DelayedCall(readySeconds, () => end.OnNext(Unit.Default), false).GetPausable().AddTo(this);

        return end;
    }
    
    public IObservable<Unit> Shoot()
    {
        collider.enabled = true;
        rigidBody.WakeUp();
        renderer.enabled = true;
        renderer.material.SetTexture("_MainTex", mainTex);

        DOVirtual.DelayedCall(0.5f, () =>
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