using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GetDPinEnemy : MonoBehaviour
{
    public event EventHandler gotDP;

    [SerializeField] float freezeSeconds = 0.25f;

    HeroMover hero;
    PolygonCollider2D col;

    ///<summary>敵のソウル的なのからDPを奪う、光ってからフェードアウトする</summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            var dPinEnemy = other.GetComponentInParent<IHaveDPinEnemy>().DPCD;
            if(dPinEnemy.IsActive)
            {
                dPinEnemy.Light();
                this.StartPausableCoroutine(FreezeAndMelt(dPinEnemy));
                float dp = dPinEnemy.CollectDP();
                gotDP?.Invoke(dp, EventArgs.Empty);
            }
        }
    }

    IEnumerator FreezeAndMelt(DPinEnemy die)
    {
        var colReversed = CameraController.CurrentCamera.AfterEffects.Find("ColorReversed");
        var noise       = CameraController.CurrentCamera.AfterEffects.Find("Noise");

        yield return new WaitForSecondsRealtime(0.05f);

        hero.TimeManager.SetTimeScale(TimeEffectType.GetDP, 0);
        colReversed.SetActive(true);
        noise.SetActive(false);

        yield return new WaitForSecondsRealtime(freezeSeconds);

        hero.TimeManager.SetTimeScale(TimeEffectType.GetDP, 1);
        colReversed.SetActive(false);
        noise.SetActive(true);
        die.FadeOut();
    }

    void Start()
    {
        hero = GetComponentInParent<HeroMover>();
        col  = GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate()
    {
        Vector2 lastPos    = (HeroDefiner.PastPoss[1] != null ? HeroDefiner.PastPoss[1] : new Vector3());
        Vector2 currentPos = (HeroDefiner.PastPoss[0] != null ? HeroDefiner.PastPoss[0] : new Vector3());
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

public interface IHaveDPinEnemy
{
    DPinEnemy DPCD{ get; }
}
