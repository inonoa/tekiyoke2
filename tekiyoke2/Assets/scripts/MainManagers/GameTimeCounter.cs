using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;

public class GameTimeCounter : MonoBehaviour
{
    public static GameTimeCounter CurrentInstance{ get; private set; }

    public float Seconds{ get; set; } = 0;
    public bool DoesTick{ get; set; } = true;

    [SerializeField] Image[] numImages;
    [SerializeField] Sprite[] numSprites;
    [SerializeField] float watchRadius = 300;
    [SerializeField] float digit2digit = 25;

    void Awake(){
        CurrentInstance = this;
    }

    void Start()
    {
        for(int i=0;i<8;i++){
            numImages[i].material = new Material(numImages[i].material);
        }
        for(int i=0;i<8;i++){
            numImages[i].material.SetFloat("_CenterX", digit2digit * (i - 3.5f));
            numImages[i].material.SetFloat("_Radius", watchRadius);
        }
    }

    Sprite Char2NumSprite(char dgt){
        switch(dgt){

            case '0': return numSprites[0];
            case '1': return numSprites[1];
            case '2': return numSprites[2];
            case '3': return numSprites[3];
            case '4': return numSprites[4];
            case '5': return numSprites[5];
            case '6': return numSprites[6];
            case '7': return numSprites[7];
            case '8': return numSprites[8];
            case '9': return numSprites[9];

            case ':': return numSprites[10]; //区切りは':'
            default : return numSprites[15];
        }
    }

    void Update()
    {
        int mins = ((int)Seconds) / 60 % 99; //UIに2けたしか出せない
        int secs = ((int)Seconds) % 60;
        int csec = (int)(Seconds % 1 * 100);

        string timeStr = mins.ToString("00") + ":" + secs.ToString("00") + ":" + csec.ToString("00");

        for(int i=0;i<8;i++){
            numImages[i].sprite = Char2NumSprite(timeStr[i]);
        }

        if(DoesTick) Seconds += TimeManager.Current.DeltaTimeExceptHero; //AroundHeroにするとDraftMode中はスローになるけど、まあこっちの方が面白いでしょ
    }
}
