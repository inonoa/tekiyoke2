using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RecoverItem : MonoBehaviour
{
    Sequence waveSeq;

    [SerializeField] SpriteRenderer gazo;
    [SerializeField] float wavePeriod = 1;
    [SerializeField] float waveWidth = 20;

    void Start()
    {
        gazo.transform.position -= new Vector3(0, waveWidth / 2, 0);
        waveSeq = DOTween.Sequence();
        waveSeq.Append(
            gazo.transform.DOMoveY(waveWidth, wavePeriod / 2)
            .SetRelative()
            .SetEase(Ease.InOutSine)
        );
        waveSeq.Append(
            gazo.transform.DOMoveY(-waveWidth, wavePeriod / 2)
            .SetRelative()
            .SetEase(Ease.InOutSine)
        );
        waveSeq.SetLoops(-1);
    }

    void OnDisable() => waveSeq.Pause();
    void OnEnable() => waveSeq?.Play();

    ///<summary></summary>
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Player"){
            Destroy(this.gameObject);
            HeroDefiner.currentHero.hpcntr.ChangeHP(HeroDefiner.currentHero.hpcntr.HP + 1);
        }
    }
}
