using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MyMath
{
    public static float DistanceXY(Vector3 vec1, Vector3 vec2){
        return (float) Math.Sqrt( (vec1.x - vec2.x) * (vec1.x - vec2.x) + (vec1.y - vec2.y) * (vec1.y - vec2.y) );
    }

    public static float FloorAndCeil(float floor, float x , float ceil) => Math.Max(floor, Math.Min(x, ceil));

    ///<summary>-1%3が-1を返してくるので、(-1).Mod(3)が2を返すように作った</summary>
    public static int Mod(this int n, int m){
        //正になるまでmを足していけばいい
        if(n < 0) return n + ((-n)/m + 1) * m;
        else      return n % m;
    }

    public static Vector2 ToVector2(this Vector3 vec3) => new Vector2(vec3.x, vec3.y);
    public static Vector3 ToVector3(this Vector2 vec2) => new Vector3(vec2.x, vec2.y, 0);
}
