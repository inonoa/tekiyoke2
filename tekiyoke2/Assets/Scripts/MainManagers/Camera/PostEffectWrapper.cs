using UnityEngine;
using System;
using UnityEngine.Rendering;

[Serializable]
public class PostEffectWrapper : INamable {
    public bool IsActive{
        get => _IsActive;
        set{
            if(value!=_IsActive){
                if(value) ApplyCommandBuf();
                else      RemoveCommandBuf();
            }
            _IsActive = value;
        }
    }
    [SerializeField] bool _IsActive = false;

    [SerializeField] Material _Material;
    public Material Material => _Material;
    public string Name => Material.name;

    [SerializeField] string volumePropertyName;
    [SerializeField] float defaultVolume;

    public void SetVolume(float volumeRate) => Material.SetFloat(volumePropertyName, defaultVolume * volumeRate);
    public float GetVolume() => Material.GetFloat(volumePropertyName) / defaultVolume;

    public void Init(Camera cmr){
        this.camera = cmr;
        this.buffer = CreateCommandBuf();
        if(IsActive) ApplyCommandBuf();
    }
    CommandBuffer buffer;
    Camera camera;
    void ApplyCommandBuf(){
        camera.AddCommandBuffer(CameraEvent.AfterEverything, buffer);
    }
    void RemoveCommandBuf(){
        camera.RemoveCommandBuffer(CameraEvent.AfterEverything, buffer);
    }
    CommandBuffer CreateCommandBuf(){

        CommandBuffer cBuffer = new CommandBuffer();

        int tmpRTID = Shader.PropertyToID("TmpRT" + Name);
        cBuffer.GetTemporaryRT(tmpRTID, -1, -1, 0, FilterMode.Bilinear);

        cBuffer.Blit(
            BuiltinRenderTextureType.CurrentActive,
            tmpRTID
        );
        cBuffer.Blit(
            tmpRTID,
            BuiltinRenderTextureType.CurrentActive,
            Material
        );

        cBuffer.ReleaseTemporaryRT(tmpRTID);

        return cBuffer;
    }
}
