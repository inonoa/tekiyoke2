using UnityEngine;

public class CameraLockingArea : MonoBehaviour
{
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;

    public bool LockX => lockX;
    public bool LockY => lockY;
}