using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class MaterialTween
{
    public static Tween To(this Material material, string propertyName, float to, float duration)
    {
        int propId = Shader.PropertyToID(propertyName);
        return DOTween.To
        (
            () => material.GetFloat(propId),
            v  => material.SetFloat(propId, v),
            to,
            duration
        );
    }
}
