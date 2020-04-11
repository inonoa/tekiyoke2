using UnityEngine;
using System;

[Serializable]
public class PostEffectWrapper {

    public bool isActive = false;

    [SerializeField] Material material;
    public Material Material => material;
    [SerializeField] string volumePropertyName;
    [SerializeField] float defaultVolume;

    public void SetVolume(float volumeRate) => material.SetFloat(volumePropertyName, defaultVolume * volumeRate);
    public float GetVolume() => material.GetFloat(volumePropertyName) / defaultVolume;
}
