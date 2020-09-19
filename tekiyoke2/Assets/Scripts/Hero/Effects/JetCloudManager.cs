using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class JetCloudManager : MonoBehaviour
{
    void Start(){
        DOTween.SetTweensCapacity(200, 125);
        
        cloudsDefaultPosX = clouds.Select( sr => sr.transform.localPosition.x ).ToArray();
        seqs = new Sequence[clouds.Length];
        endSeqs = new Sequence[clouds.Length];
    }
    


    [SerializeField] SpriteRenderer[] clouds;
    [SerializeField] float[] cloudsDstX;
    float[] cloudsDefaultPosX;
    Sequence[] seqs;
    Sequence[] endSeqs;
    [SerializeField] float durationSec;

    public void StartClouds(){

        foreach(Sequence sq in endSeqs){
            sq?.Kill();
        }

        for(int i=0; i<clouds.Length; i++){

            seqs[i] = DOTween.Sequence();
            seqs[i].Append(
                clouds[i].transform.DOLocalMoveX(cloudsDstX[i], durationSec).SetEase(Ease.OutSine)
            );
            seqs[i].Join(
                clouds[i].DOFade(1, durationSec / 2)
            );
        }
    }

    public void EndClouds(){
        foreach(Sequence sq in seqs){
            sq?.Kill();
        }

        for(int i=0; i<clouds.Length; i++){

            endSeqs[i] = DOTween.Sequence();
            endSeqs[i].Append(
                clouds[i].transform.DOLocalMoveX(cloudsDefaultPosX[i], durationSec / 3)
            );
            endSeqs[i].Join(
                clouds[i].DOFade(0, durationSec / 3)
            );
        }

    }
}
