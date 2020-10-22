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
    [SerializeField] Image failDialogImg;
    [SerializeField] SoundGroup soundGroup;

    public void Try2Save(){
        if(DPManager.Instance.DP >= saveCostDP){
            
            DPManager.Instance.UseDP(saveCostDP);
            //MemoryOverDeath.Instance.Save();
            resPos.SetActive(true);
            resPos.transform.position = new Vector3(transform.position.x, transform.position.y, resPos.transform.position.z);

            soundGroup.Play("Save");

            FadeInOut(dialogImg, 200);

        }else{
            FadeInOut(failDialogImg, 50);
        }

        void FadeInOut(Image img, float moveX){

            img.color = new Color(1,1,1,0);
            { Vector3 pos = img.transform.localPosition; pos.x = moveX; img.transform.localPosition = pos; }

            Sequence dialogSeq = DOTween.Sequence();

            dialogSeq.Append(img.DOFade(1, 0.5f));
            dialogSeq.Join  (img.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutSine));

            dialogSeq.Append(img.transform.DOLocalMoveX(0, 1f));

            dialogSeq.Append(img.DOFade(0, 0.5f));
            dialogSeq.Join  (img.transform.DOLocalMoveX(-moveX, 0.5f).SetEase(Ease.InSine));
        }
    }

    void Start()
    {
        resPos = DraftManager.CurrentInstance.GameMasterTF.Find("RespawnPosition").gameObject;
    }
}
