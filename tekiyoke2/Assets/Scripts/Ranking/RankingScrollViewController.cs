using System;
using System.Collections.Generic;
using DG.Tweening;
using Ranking;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RankingScrollViewController : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] FocusNode focusNode;
    [SerializeField] Material nodesMat;
    
    [SerializeField] float scrollSpeedMax = 200f;
    [SerializeField] float scrollForce = 100f;
    [SerializeField] float resistanceRate = 5;


    IReadOnlyList<RankNodeView> nodes;
    public void OnNodesSet(IReadOnlyList<RankNodeView> nodes)
    {
        this.nodes = nodes;
    }

    Tween blink;
    void OnFocused()
    {
        nodes?.ForEach(node => node.OnFocused());

        blink = DOTween.Sequence()
            .Append(nodesMat.To("_DPAlpha", 0.3f, 0.3f).SetEase(Ease.Linear))
            .AppendInterval(1f)
            .Append(nodesMat.To("_DPAlpha", 0, 0.3f).SetEase(Ease.Linear))
            .AppendInterval(1.5f)
            .SetLoops(-1);
    }

    void OnUnFocused()
    {
        blink?.Kill();
        nodes?.ForEach(node => node.OnUnFocused());
        nodesMat.To("_DPAlpha", 0, 0.3f).SetEase(Ease.Linear);
    }

    void Awake()
    {
        focusNode.OnFocused.Subscribe(_ => OnFocused());
        focusNode.OnUnFocused.Subscribe(_ => OnUnFocused());
        nodesMat.SetFloat("_DPAlpha", 0);
    }

    void Update()
    {
        if(!focusNode.Focused) return;

        float dt = Time.deltaTime;
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            scrollRect.velocity += Vector2.down * scrollForce * dt;
            if (scrollRect.velocity.y < -scrollSpeedMax)
            {
                scrollRect.velocity = new Vector2(0, -scrollSpeedMax);
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            scrollRect.velocity += Vector2.up * scrollForce * dt;
            if (scrollRect.velocity.y > scrollSpeedMax)
            {
                scrollRect.velocity = new Vector2(0, scrollSpeedMax);
            }
        }
        else if(scrollRect.velocity.y > 0)
        {
            scrollRect.velocity += Vector2.down * scrollForce * dt * resistanceRate;
            if (scrollRect.velocity.y < 0)
            {
                scrollRect.velocity = Vector2.zero;
            }
        }
        else if (scrollRect.velocity.y < 0)
        {
            scrollRect.velocity += Vector2.up * scrollForce * dt * resistanceRate;
            if (scrollRect.velocity.y > 0)
            {
                scrollRect.velocity = Vector2.zero;
            }
        }
    }
}
