using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UniRx;

public class SimpleAnim : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;
    Tween[] tweensToRestart;
    [SerializeField] float changeSec = 0.1f;

    public void ResetAndStartAnim(TweenCallback onComplete = null)
    {
        foreach(Tween tw in tweensToRestart) tw?.Kill();

        spriteRenderer.sprite = sprites[0];
        for(int i = 0; i < sprites.Length - 1; i++)
        {
            int i_copy = i; //コピーしないとループが回りきってからiの値を使用するので配列外参照起こす

            tweensToRestart[i_copy] = DOVirtual.DelayedCall
            (
                (i_copy+1) * changeSec,
                () => spriteRenderer.sprite = sprites[i_copy+1],
                ignoreTimeScale: false
            )
            .AsHeros();
        }
        tweensToRestart[sprites.Length-1] = DOVirtual.DelayedCall(sprites.Length * changeSec, onComplete, ignoreTimeScale: false).AsHeros();
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tweensToRestart = new Tween[sprites.Length];
    }
}
