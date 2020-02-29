using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SavePositionManager : MonoBehaviour
{
    GameObject resPos;

    [SerializeField]
    int saveCostDP = 10;
    [SerializeField] Image dialogImg;

    public void Try2Save(){
        if(DPManager.Instance.DP >= saveCostDP){
            
            DPManager.Instance.UseDP(saveCostDP);
            MemoryOverDeath.Instance.Save();
            resPos.SetActive(true);
            resPos.transform.position = new Vector3(transform.position.x, transform.position.y, resPos.transform.position.z);

            //応急
            Sequence dialogSeq = DOTween.Sequence();

            dialogSeq.Append(dialogImg.DOFade(1, 0.5f));
            dialogSeq.Join  (dialogImg.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutSine));

            dialogSeq.Append(dialogImg.transform.DOLocalMoveX(0, 1f));

            dialogSeq.Append(dialogImg.DOFade(0, 0.5f));
            dialogSeq.Join  (dialogImg.transform.DOLocalMoveX(-100, 0.5f).SetEase(Ease.InSine));

            dialogSeq.Append(dialogImg.transform.DOLocalMoveX(100, 0f));

        }else{
            print("セーブに失敗しました。");
        }
    }

    void Start()
    {
        resPos = transform.parent.Find("RespawnPosition").gameObject;
    }
}
