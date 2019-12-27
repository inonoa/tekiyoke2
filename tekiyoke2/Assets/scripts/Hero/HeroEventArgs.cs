using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroJumpedEventArgs : EventArgs
{
    public HeroJumpedEventArgs(bool isFromGround, bool isKick){
        this.isFromGround = isFromGround;
        this.isKick = isKick;
    }

    public bool isKick = false;
    public bool isFromGround = false;
}
