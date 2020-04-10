using UnityEngine;
using System;

public class PostEffectWrapper {

    public bool IsActive{ get; private set; } = false;

    public event EventHandler ActiveChanged;
    public void SetActive(bool val){
        IsActive = val;
        ActiveChanged?.Invoke(val, EventArgs.Empty);
    }

    public readonly Material material;
    public readonly string volumePropertyName;
    public readonly float defaultVolume;

    public PostEffectWrapper(Material mat, string volumePropertyName, float defaultVolume, bool IsActive = false){
        (this.material, this.volumePropertyName, this.defaultVolume, this.IsActive) = (mat, volumePropertyName, defaultVolume, IsActive);
    }

    public void SetVolume(float volumeRate){
        material.SetFloat(volumePropertyName, defaultVolume * volumeRate);
    }

    public float GetVolume() => material.GetFloat(volumePropertyName) / defaultVolume;
}
