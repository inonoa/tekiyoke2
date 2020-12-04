using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RankingScrollViewController : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] FocusNode focusNode;
    
    [SerializeField] float scrollSpeedMax = 200f;
    [SerializeField] float scrollForce = 100f;
    [SerializeField] float resistanceRate = 5;

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
