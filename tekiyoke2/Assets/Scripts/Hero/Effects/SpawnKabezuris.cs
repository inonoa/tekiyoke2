using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnKabezurisExtension
{
    public static IEnumerator SpawnKabezuris(this HeroMover hero, MoveInAirParams params_)
    {
        Try2SpawnKabezuri(hero);

        while(true)
        {
            yield return new WaitForSeconds(params_.KabezuriInterval / TimeManager.CurrentInstance.TimeScaleAroundHero);

            Try2SpawnKabezuri(hero);
        }
    }

    static void Try2SpawnKabezuri(HeroMover hero)
    {
        if(hero.velocity.Y > 0) return;

        bool dir_is_R;

        if(hero.CanKickFromWallR && hero.CanKickFromWallL) dir_is_R = hero.WantsToGoRight;
        else if(hero.CanKickFromWallR)                     dir_is_R = true;
        else if(hero.CanKickFromWallL)                     dir_is_R = false;
        else return;

        hero.ObjsHolderForStates.KabezuriPool.ActivateOne(dir_is_R ? "r" : "l");
    }
}
