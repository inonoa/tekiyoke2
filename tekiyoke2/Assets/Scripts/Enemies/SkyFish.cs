using Sirenix.OdinInspector;
using UnityEngine;

public class SkyFish : MonoBehaviour, IHaveDPinEnemy
{
    [field: SerializeField, LabelText(nameof(DPCD))]
    public DPinEnemy DPCD { get; private set; }
}