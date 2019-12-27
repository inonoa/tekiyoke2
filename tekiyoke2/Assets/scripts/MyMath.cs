using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyMath
{
    public static float DistanceXY(Vector3 vec1, Vector3 vec2){
        return (float) Math.Sqrt( (vec1.x - vec2.x) * (vec1.x - vec2.x) + (vec1.y - vec2.y) * (vec1.y - vec2.y) );
    }

    public static float FloorAndCeil(float floor, float x , float ceil) => Math.Max(floor, Math.Min(x, ceil));
}
