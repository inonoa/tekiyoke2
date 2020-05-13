using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresToText : MonoBehaviour
{
    [SerializeField] Image[] timeImages;
    [SerializeField] Image[] bestTimeImages;
    void Start()
    {
        var scores = ScoreHolder.Instance;

        int lastDraft = SceneTransition.LastStageIndex();

        float timeLast = (lastDraft!=-1 ? scores.clearTimesLast[lastDraft] : 55 * 60 + 55.55f);
        int[] timeDigitsLast = TimeFloat2Ints(timeLast);

        float timeBest = (scores.BestTimeExists(lastDraft) ? scores.clearTimesBestExceptLast[lastDraft] : 99 * 60 + 59.995f);
        int[] timeDigitsBest = TimeFloat2Ints(timeBest);

        for(int i=0;i<6;i++){
            timeImages[i].sprite = NumberSpritesHolder.Instance.NumberSprites[timeDigitsLast[i]];
            bestTimeImages[i].sprite = NumberSpritesHolder.Instance.NumberSprites[timeDigitsBest[i]];
        }
    }

    ///<summary>ツイートとかに使うのでStart時にはApplyできない</summary>
    void OnDestroy() => ScoreHolder.Instance.ApplyLastTime2Best();

    int[] TimeFloat2Ints(float timeSecs){
        int[] digits = new int[6];

        int csc = (int)((timeSecs - (int)timeSecs) * 100);
        digits[4] = csc / 10;
        digits[5] = csc % 10;

        int sec = ((int)timeSecs)%60;
        digits[2] = sec / 10;
        digits[3] = sec % 10;

        int min = ((int)timeSecs) / 60 % 100;
        digits[0] = min / 10;
        digits[1] = min % 10;

        return digits;
    }

}
