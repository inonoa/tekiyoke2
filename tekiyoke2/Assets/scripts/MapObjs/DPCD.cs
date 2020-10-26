using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DPCD : MonoBehaviour
{
    [SerializeField] float rotateDegreePerSec = 120;
    [SerializeField] int DPperDPCD = 1;

    bool gotten = false;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer lightRenderer;

    void Update()
    {
        float dt = TimeManager.CurrentInstance.DeltaTimeExceptHero;
        transform.Rotate(new Vector3(0, 0, gotten ? rotateDegreePerSec * dt * 2 : rotateDegreePerSec * dt));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !gotten)
        {
            gotten = true;
            
            HeroMover hero = other.GetComponentInParent<HeroMover>();
            hero.GetDP(DPperDPCD);

            GottenAnimation();
            GetComponent<SoundGroup>().Play("Got");
        }
    }

    void GottenAnimation()
    {
        Sequence gottenSeq = DOTween.Sequence();

        float fadeSec = 0.1f;
        gottenSeq.Append(lightRenderer.DOFade(1,fadeSec));
        float lightSec = 0.1f;
        gottenSeq.AppendInterval(lightSec);
        float dieSec = 0.2f;
        gottenSeq.Append(lightRenderer.DOFade(0, dieSec));
        gottenSeq.Join(spriteRenderer.DOFade(0, dieSec));
        gottenSeq.OnComplete(() => Destroy(gameObject));
    }
}
