using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class MaterialTween
{
    public static Tween To(this Material material, string propertyName, float to, float duration)
    {
        int propId = Shader.PropertyToID(propertyName);
        return material.To(propId, to, duration);
    }
    
    public static Tween To(this Material material, int propertyID, float to, float duration)
    {
        return DOTween.To
        (
            () => material.GetFloat(propertyID),
            v  => material.SetFloat(propertyID, v),
            to,
            duration
        );
    }
}
