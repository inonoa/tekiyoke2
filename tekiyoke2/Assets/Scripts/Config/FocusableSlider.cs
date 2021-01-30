using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FocusableSlider : SerializedMonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] FocusNode node;
    [SerializeField] IInput input;

    [SerializeField] float speed;

    void Update()
    {
        if (!node.Manager.AcceptsInput) return;
        
        if (node.Focused)
        {
            if (input.GetButton(ButtonCode.Right))
            {
                slider.value += speed * Time.deltaTime;
            }

            if (input.GetButton(ButtonCode.Left))
            {
                slider.value -= speed * Time.deltaTime;
            }
        }
    }
}