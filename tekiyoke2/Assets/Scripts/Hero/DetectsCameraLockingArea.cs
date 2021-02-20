using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class DetectsCameraLockingArea : MonoBehaviour
{
    [field: SerializeField, ReadOnly, LabelText(nameof(LockedBy))]
    public CameraLockingArea LockedBy { get; private set; }
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.CameraLockingArea))
        {
            LockedBy = other.GetComponent<CameraLockingArea>();
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.CameraLockingArea))
        {
            LockedBy = null;
        }
    }
}