using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GetDPinEnemy : MonoBehaviour
{
    public event EventHandler gotDP;

    [SerializeField] int freezeFrames = 7;

    HeroMover hero;
    PolygonCollider2D col;

    ///<summary>敵のソウル的なのからDPを奪う、光ってからフェードアウトする</summary>
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Enemy"){
            var dPinEnemy = other.GetComponentInParent<EnemyController>().DPCD;
            if(dPinEnemy.IsActive){
                dPinEnemy.Light();
                StartCoroutine(FreezeAndMelt(dPinEnemy));
                float dp = dPinEnemy.CollectDP();
                gotDP?.Invoke(dp, EventArgs.Empty);
            }
        }
    }

    IEnumerator FreezeAndMelt(DPinEnemy die){
        yield return new WaitForSecondsRealtime(0.05f);

        Tokitome.SetTime(0);
        var colReversed = CameraController.CurrentCamera.AfterEffects.Find("ColorReversed");
        var noise = CameraController.CurrentCamera.AfterEffects.Find("Noise");
        colReversed.IsActive = true;
        noise.IsActive = false;
        for(int i=0; i<freezeFrames-1; i++){
            yield return null;
        }
        Tokitome.SetTime(1);
        colReversed.IsActive = false;
        noise.IsActive = true;
        die.FadeOut();
    }

    void Start(){
        hero = GetComponentInParent<HeroMover>();
        col = GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate(){
        Vector2 lastPos = (HeroDefiner.CurrentHeroPastPos[1] != null ? HeroDefiner.CurrentHeroPastPos[1] : new Vector3());
        Vector2 currentPos = (HeroDefiner.CurrentHeroPastPos[0] != null ? HeroDefiner.CurrentHeroPastPos[0] : new Vector3());
        Vector2 posDist = currentPos - lastPos;

        Vector2[] colPoints = new Vector2[4]{
            new Vector2(0,25),
            new Vector2(0,-25),
            new Vector2(0,-25) - posDist,
            new Vector2(0,25) - posDist
        };
        col.SetPath(0, colPoints);
    }
}
