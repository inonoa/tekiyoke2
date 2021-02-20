using UnityEngine;

[CreateAssetMenu(fileName = "WarpParams", menuName = "ScriptableObject/WarpParams", order = 0)]
public class WarpParams : ScriptableObject
{
    [SerializeField] float cameraFreezeSeconds;
    public float CameraFreezeSeconds => cameraFreezeSeconds;
}