using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresToText : MonoBehaviour
{
    [SerializeField] Image[] timeImages;
    [SerializeField] Image[] bestTimeImages;

    [SerializeField] NumberSpritesHolder numSpritesHolder;
    
    public void Init(int stage, float time, bool isFirstPlay, float lastBestTime)
    {
        float timeLast = (stage != -1 ? time : 55 * 60 + 55.55f);
        int[] timeDigitsLast = TimeFloat2Ints(timeLast);

        int[] timeDigitsBest = float.IsPositiveInfinity(lastBestTime) ? new []{10, 10, 10, 10, 10, 10} : TimeFloat2Ints(lastBestTime);

        for(int i = 0; i < 6; i++)
        {
            timeImages[i].sprite     = numSpritesHolder.NumberSprites[timeDigitsLast[i]];
            bestTimeImages[i].sprite = numSpritesHolder.NumberSprites[timeDigitsBest[i]];
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
