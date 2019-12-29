using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpeedResidue
{
    float SpeedX{ get; }
    float SpeedY{ get; }

    ///<summary>捨てられたい場合にtrueを返してください！！</summary>
    bool UpdateSpeed(HeroMover hero);
}
