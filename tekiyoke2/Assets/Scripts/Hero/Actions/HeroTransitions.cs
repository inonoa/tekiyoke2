using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeroTransitions
{
    public static bool IsReady2Kick2Left(this HeroMover hero, IAskedInput input)
    {
        ButtonCode arrowbutton = hero.Parameters.KickParams.KickKey == KickKey.DirOfWall ?
                                   ButtonCode.Right
                                 : ButtonCode.Left;
        
        return    input.GetButton(arrowbutton)
               && input.GetButtonDown(ButtonCode.Jump)
               && hero.CanKickFromWallR;
    }
    public static bool IsReady2Kick2Right(this HeroMover hero, IAskedInput input)
    {
        ButtonCode arrowbutton = hero.Parameters.KickParams.KickKey == KickKey.DirOfWall ?
                                   ButtonCode.Left
                                 : ButtonCode.Right;
        
        return    input.GetButton(arrowbutton)
               && input.GetButtonDown(ButtonCode.Jump)
               && hero.CanKickFromWallL;
    }
}
