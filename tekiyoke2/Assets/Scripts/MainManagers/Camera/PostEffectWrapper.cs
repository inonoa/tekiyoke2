using UnityEngine;
using System;
using UnityEngine.Rendering;
using UniRx;

[Serializable]
public class PostEffectWrapper
{
    bool commandBufApplied = false;
    public bool IsActive => _IsActive;
    [SerializeField] bool _IsActive = false;
    public void SetActive(bool value)
    {
        if(value != _IsActive)
        {
            _IsActive = value;
            _BecomeDirty.OnNext(Unit.Default);
        }
    }

    readonly Subject<Unit> _BecomeDirty = new Subject<Unit>();
    public IObservable<Unit> BecomeDirty => _BecomeDirty;

    [SerializeField] Material _Material;
    public Material Material => _Material;
    public string Name => Material.name;

    [SerializeField] string volumePropertyName;
    [SerializeField] float defaultVolume;

    public void SetVolume(float volumeRate) => Material.SetFloat(volumePropertyName, defaultVolume * volumeRate);
    public float GetVolume() => Material.GetFloat(volumePropertyName) / defaultVolume;

    public void Init(Camera cmr)
    {
        this.camera = cmr;
        this.buffer = CreateCommandBuf();
    }
    CommandBuffer buffer;
    Camera camera;
    public void ApplyCommandBuf()
    {
        camera.AddCommandBuffer(CameraEvent.AfterEverything, buffer);
        commandBufApplied = true;
    }
    public void RemoveCommandBuf()
    {
        camera.RemoveCommandBuffer(CameraEvent.AfterEverything, buffer);
        commandBufApplied = false;
    }
    CommandBuffer CreateCommandBuf()
    {
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

    public void Update_()
    {
        if(commandBufApplied != IsActive)
        {
            _BecomeDirty.OnNext(Unit.Default);
        }
    }
}
