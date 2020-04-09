using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryOverDeath
{
    float time = 0;
    (float x, float y) heroPosition = (0,0);

    public void Save(){
        time = GameTimeCounter.CurrentInstance.Seconds;
        heroPosition = (HeroDefiner.CurrentHeroPos.x, HeroDefiner.CurrentHeroPos.y);
    }

    public void SaveOnDeath(){
        time = GameTimeCounter.CurrentInstance.Seconds;
    }

    public void Load(){
        GameTimeCounter.CurrentInstance.Seconds = time;
        GameTimeCounter.CurrentInstance.DoesTick = true;
        //今は初期位置でセーブする意味が無いのであれだがしょうらい……
        if(heroPosition!=(0,0)){
            HeroDefiner.currentHero.WarpPos(heroPosition.x, heroPosition.y);
            GameObject resPos = DraftManager.CurrentInstance.GameMasterTF.transform.Find("RespawnPosition").gameObject; //苦しい
            resPos.SetActive(true);
            resPos.transform.position = new Vector3(heroPosition.x, heroPosition.y, resPos.transform.position.z);
        }
    }

    public void Clear(){
        time = 0;
        heroPosition = (0, 0);
    }


    #region Singleton
    static MemoryOverDeath _Instance;
    public static MemoryOverDeath Instance{
        get => (_Instance!=null ? _Instance : _Instance = new MemoryOverDeath());
    }
    private MemoryOverDeath(){ }
    #endregion
}
