using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresToText : MonoBehaviour
{
    [SerializeField] Image[] timeImages;
    [SerializeField] Image[] bestTimeImages;
    [SerializeField] ScoreHolder scoreHolder;
    [SerializeField] SaveDataManager saveDataManager;
    void Start()
    {
        StagePlayData playData = scoreHolder.Get();
        (bool isFirstPlay, float lastBestTime) = saveDataManager.SetStageData(playData);

        float timeLast = (playData.Stage != -1 ? playData.Time : 55 * 60 + 55.55f);
        int[] timeDigitsLast = TimeFloat2Ints(timeLast);

        float timeBest = (lastBestTime);
        int[] timeDigitsBest = TimeFloat2Ints(timeBest);

        for(int i = 0; i < 6; i++)
        {
            timeImages[i].sprite     = NumberSpritesHolder.Instance.NumberSprites[timeDigitsLast[i]];
            bestTimeImages[i].sprite = NumberSpritesHolder.Instance.NumberSprites[timeDigitsBest[i]];
        }
    }

    int[] TimeFloat2Ints(float timeSecs)
    {
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
