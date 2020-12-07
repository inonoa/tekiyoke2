using System;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Kone : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [field: SerializeField, LabelText(nameof(DPCD))]
    public DPinEnemy DPCD { get; private set; }

    [SerializeField] Collider2D heroSensor;

    bool nearHero;
    
    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Awake()
    {
        heroSensor.OnTriggerEnter2DAsObservable()
            .Where(other => other.CompareTag(TagNames.Hero))
            .Subscribe(heroCol => nearHero = true);
        heroSensor.OnTriggerExit2DAsObservable()
            .Where(other => other.CompareTag(TagNames.Hero))
            .Subscribe(heroCol => nearHero = false);
    }

    void Update()
    {
        if(nearHero) transform.Rotate(0, 0, 1);
    }
}