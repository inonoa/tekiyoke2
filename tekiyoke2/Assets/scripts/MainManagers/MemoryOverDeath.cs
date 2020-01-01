using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryOverDeath
{
    float time;
    string checkPoint;
    (float x, float y) heroPosition;

    public void Save(){
        time = GameTimeCounter.CurrentInstance.count;
        checkPoint = "とりあえず";
    }

    public void SavePosition(){
        heroPosition = (HeroDefiner.CurrentHeroPos.x, HeroDefiner.CurrentHeroPos.y);
        Debug.Log(heroPosition);
    }

    public void Load(){
        GameTimeCounter.CurrentInstance.count = time;
        GameTimeCounter.CurrentInstance.DoesTick = true;
        HeroDefiner.currentHero.WarpPos(heroPosition.x, heroPosition.y);
    }


    #region Singleton
    static MemoryOverDeath _Instance;
    public static MemoryOverDeath Instance{
        get => (_Instance!=null ? _Instance : _Instance = new MemoryOverDeath());
    }
    private MemoryOverDeath(){ }
    #endregion
}
