using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DPinEnemy : MonoBehaviour
{
    public bool IsActive{ get; private set; } = true;
    [SerializeField] float dp = 1;
    [SerializeField] float rotateSpeed = 1;
    readonly float lightDurationFrames = 6;
    readonly float fadeoutDuration = 0.5f;

    SpriteRenderer spriteRenderer;

    new ShaderPropertyFloat light;

    public float CollectDP(){
        IsActive = false;
        return dp;
    }

    public void Light(){
        if(IsActive) StartCoroutine(LightCoroutine());
    }

    IEnumerator LightCoroutine(){

        for(int i=0; i<lightDurationFrames; i++){
            light.SetVal((i+1) / lightDurationFrames);
            yield return null;
        }
    }

    public void FadeOut(){
        if(IsActive) spriteRenderer.DOFade(0, fadeoutDuration);
    }


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        light = new ShaderPropertyFloat(spriteRenderer.material, "_Volume");

        DOVirtual.DelayedCall(10f, Light);
        DOVirtual.DelayedCall(15f, FadeOut);
    }


    void Update()
    {
        if(IsActive){
            transform.Rotate(0,0,rotateSpeed);
            spriteRenderer.color = new Color(1,1,1,
                Mathf.Clamp01((200 -  MyMath.DistanceXY(HeroDefiner.CurrentHeroPos, transform.position)) / 200)
            );
        }
    }
}
