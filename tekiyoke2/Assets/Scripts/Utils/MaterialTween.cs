using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class MaterialTween
{
    public static Tween To(this Material material, string propertyName, float to, float duration)
    {
        return DOTween.To(
            () => material.GetFloat(propertyName),
            v  => material.SetFloat(propertyName, v),
            to,
            duration
        );
    }
}
