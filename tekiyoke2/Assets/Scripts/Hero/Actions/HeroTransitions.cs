using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class HeroTransitions
{
    public static bool IsReady2Kick2Left(this HeroMover hero, IAskedInput input)
    {
        if (!input.GetButtonDown(ButtonCode.Jump)) return false;
        if (!hero.CanKickFromWallR) return false;
        
        switch (hero.Parameters.KickParams.KickKey)
        {
            case KickKey.DirAgainstWall:
                return input.GetButton(ButtonCode.Left);
            case KickKey.DirOfWall:
                return input.GetButton(ButtonCode.Right);
            case KickKey.Any:
                return true;
            default:
                throw new NotImplementedException();
        }
    }
    public static bool IsReady2Kick2Right(this HeroMover hero, IAskedInput input)
    {
        if (!input.GetButtonDown(ButtonCode.Jump)) return false;
        if (!hero.CanKickFromWallL) return false;
        
        switch (hero.Parameters.KickParams.KickKey)
        {
            case KickKey.DirAgainstWall:
                return input.GetButton(ButtonCode.Right);
            case KickKey.DirOfWall:
                return input.GetButton(ButtonCode.Left);
            case KickKey.Any:
                return true;
            default:
                throw new NotImplementedException();
        }
    }
}
