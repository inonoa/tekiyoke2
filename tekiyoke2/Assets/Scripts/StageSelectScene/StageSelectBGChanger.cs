using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectBGChanger : MonoBehaviour
{
    [SerializeField] Image bg;
    [SerializeField] Image bgbg; //クロスフェード用に背後に映すやつ
    [SerializeField] Image anmaku;
    [SerializeField] Sprite[] bgSprites;

    [SerializeField] float changeDuration = 0.4f;

    public void OnChangeStage(int stageIndex)
    {
        Debug.Assert(stageIndex == 0 || stageIndex == 1 || stageIndex == 2);

        bgbg.sprite = bgSprites[stageIndex];
        
        bg.DOFade(0, changeDuration)
            .SetEase(Ease.Linear)
            .onComplete += () =>
        {
            bg.sprite = bgbg.sprite;
            bg.DOFade(1, 0);
        };
        anmaku.DOFade(0.5f, changeDuration / 2)
            .onComplete += () =>
        {
            anmaku.DOFade(0, changeDuration / 2);
        };
    }
}
