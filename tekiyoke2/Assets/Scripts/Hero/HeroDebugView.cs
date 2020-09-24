using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDebugView : MonoBehaviour
{
    HeroMover hero;
    [SerializeField] HeroLogParams params_;
    
    void Start()
    {
        hero = GetComponent<HeroMover>();
    }
    
    void Update()
    {
        string txt = "";
        if(params_.State)          txt += hero.CurrentStateStr() + "\n";
        if(params_.Velocity)       txt += "Velocity: "       + hero.velocity       + "\n";
        if(params_.KeyDirection)   txt += "KeyDirection: "   + hero.KeyDirection   + "\n";
        if(params_.WantsToGoRight) txt += "WantsToGoRight: " + hero.WantsToGoRight + "\n";
        if(params_.IsOnGround)     txt += "IsOnGround: "     + hero.IsOnGround     + "\n";

        if(txt != "") Debug.Log(txt);
    }
}
