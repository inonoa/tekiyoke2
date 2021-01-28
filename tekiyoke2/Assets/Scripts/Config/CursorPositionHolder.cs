using Sirenix.OdinInspector;
using UnityEngine;

public class CursorPositionHolder : MonoBehaviour
{
    [field: SerializeField, LabelText(nameof(CursorPosTransform))]
    public Transform CursorPosTransform { get; private set; }
}