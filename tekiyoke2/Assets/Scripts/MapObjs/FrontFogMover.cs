using System;
using UnityEngine;
using UnityEngine.UI;

public class FrontFogMover : MonoBehaviour
{
    [SerializeField] float selfSpeed = 0.1f;
    [SerializeField] float speedRateFromHero = 0.1f;
    
    [SerializeField] Image image;
    Material material;
    
    static readonly int OffsetX = Shader.PropertyToID("_OffsetX");

    void Awake()
    {
        material = image.material;
    }

    void Update()
    {
        float currentOffset = material.GetFloat(OffsetX);
        float delta = (selfSpeed + HeroDefiner.currentHero.velocity.X * speedRateFromHero) * TimeManager.Current.DeltaTimeExceptHero;
        material.SetFloat(OffsetX, currentOffset + delta);
    }
}