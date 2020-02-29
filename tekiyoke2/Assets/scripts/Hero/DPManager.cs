using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DPManager : MonoBehaviour
{
    public static DPManager Instance{ get; set; }
    RectTransform rTransform;

    public int DP{ get; private set; } = 0;

    [SerializeField]
    int displayInterval = 3;
    int frames2Display = 1;

    ///<summary>1Pずつたまっていく感じにしたいので、内部的なDPとは別に見かけのDPを用意してこれをもとに描画</summary>
    int DPonDisplay = 0;
    static readonly int maxDP = 100;

    public void AddDP(int delta){
        if(delta > 0){
            DP = Math.Min(maxDP, DP + delta);
        }
        else print("負のDPは得られません");
    }

    public bool UseDP(int dp2Use){
        if(DP >= dp2Use){
            DP -= dp2Use;
            return true;
        }else{
            return false;
        }
    }

    void Start()
    {
        Instance = this;
        rTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        frames2Display --;
        if(frames2Display==0){
            frames2Display = displayInterval;

            //急速にDPが増えたら急速に追いついてほしい
            if(DPonDisplay > DP)      DPonDisplay -= 1 + (DPonDisplay - DP) / 5;
            else if(DPonDisplay < DP) DPonDisplay += 1 + (DP - DPonDisplay) / 5;

            rTransform.sizeDelta = new Vector2(1000.0f * DPonDisplay / maxDP, 50);
        }
    }
}
