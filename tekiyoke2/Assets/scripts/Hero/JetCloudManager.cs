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
    }


    [SerializeField] SpriteRenderer[] clouds;
    [SerializeField] float[] cloudsDstX;
    float[] cloudsDefaultPosX;
    Sequence[] seqs;
    [SerializeField] float durationSec;

    public void StartClouds(){

        for(int i=0; i<clouds.Length; i++){

            seqs[i] = DOTween.Sequence();
            seqs[i].Append(
                clouds[i].transform.DOLocalMoveX(cloudsDstX[i], durationSec).SetEase(Ease.OutSine)
            );
            seqs[i].Join(
                clouds[i].DOFade(1, durationSec)
            );
        }
    }

    public void EndClouds(){
        foreach(Sequence sq in seqs){
            sq.PlayBackwards();
            sq.timeScale *= 3;
        }
    }
}
