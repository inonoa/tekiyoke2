using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DPManager : MonoBehaviour
{
    public static DPManager Instance{ get; set; }

    int DP = 0;
    static readonly int maxDP = 100;

    public void AddDP(int delta){
        if(delta > 0){
            DP = Math.Min(maxDP, DP + delta);
            print(delta + "DP、ゲットだぜ！  |  今のDPは" + DP + "Pだぜ！！");
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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
