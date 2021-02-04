using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyStatic : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [field: SerializeField, LabelText(nameof(DPCD))]
    public DPinEnemy DPCD { get; private set; }
    
    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(true);
    }
}