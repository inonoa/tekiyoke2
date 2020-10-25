using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RecoverItem : MonoBehaviour
{
    Sequence waveSeq;
    bool gotten = false;

    [SerializeField] SpriteRenderer gazoHontai;
    [SerializeField] SpriteRenderer gazoGhost;
    [SerializeField] float wavePeriod = 1;
    [SerializeField] float waveWidth = 20;

    void Start()
    {
        gazoHontai.transform.position -= new Vector3(0, waveWidth / 2, 0);
        waveSeq = DOTween.Sequence();
        waveSeq.Append(
            gazoHontai.transform.DOMoveY(waveWidth, wavePeriod / 2)
            .SetRelative()
            .SetEase(Ease.InOutSine)
        );
        waveSeq.Append(
            gazoHontai.transform.DOMoveY(-waveWidth, wavePeriod / 2)
            .SetRelative()
            .SetEase(Ease.InOutSine)
        );
        waveSeq.SetLoops(-1);
    }

    void OnDisable() => waveSeq.Pause();
    void OnEnable() => waveSeq?.Play();

    ///<summary></summary>
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Player" && !gotten){
            gotten = true;
            HeroDefiner.currentHero.RecoverHP(1);
            waveSeq.Pause();

            GottenAnimation();
            GetComponent<SoundGroup>().Play("Got");
        }
    }

    void GottenAnimation(){
        Sequence gotSeq = DOTween.Sequence();

        float fadeSec = 0.3f;
        float moveInFade = 50;
        gotSeq.Append(gazoHontai.DOFade(0, fadeSec));
        gotSeq.Join(gazoGhost.DOFade(1, fadeSec));
        gotSeq.Join(transform.DOMoveY(moveInFade, fadeSec).SetRelative().SetEase(Ease.OutSine));

        float waitSec = 0.3f;
        gotSeq.AppendInterval(waitSec);

        float dieSec = 0.3f;
        float moveInDie = 50;
        gotSeq.Append(gazoGhost.DOFade(0, dieSec));
        gotSeq.Join(transform.DOMoveY(moveInDie, dieSec).SetRelative());

        gotSeq.OnComplete(() => Destroy(gameObject));
    }
}
