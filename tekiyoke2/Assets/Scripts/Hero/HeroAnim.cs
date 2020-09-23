using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeroAnim
{
    public static void SetAnim(this HeroMover hero, string id)
    {
        string trigger = id + (hero.WantsToGoRight ? "r" : "l");
        hero.Anim.SetTrigger(trigger);
    }
}
