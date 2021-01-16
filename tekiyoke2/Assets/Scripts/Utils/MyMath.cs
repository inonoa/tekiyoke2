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

    public static float DoubleRamp(float x, float thresholdAbs){
        if(x > 0) return Math.Max(0, x - thresholdAbs);
        else      return Math.Min(0, x + thresholdAbs);
    }

    public static bool ExceedB(Vector2 vecX, Vector2 vecA, Vector2 vecB){
        if(vecA.y == vecB.y) return (vecB.x > vecA.x ? vecX.x > vecB.x : vecX.x < vecB.x);

        return (vecB.x - vecA.x) * (vecX.x - vecB.x) + (vecB.y - vecA.y) * (vecX.y - vecB.y) > 0;
    }

    public static Vector2 ToVec2(this Vector3 vec3) => new Vector2(vec3.x, vec3.y);
    public static Vector3 ToVec3(this Vector2 vec2) => new Vector3(vec2.x, vec2.y, 0);

    ///<summary>v1 - v2</summary>
    public static Vector2 DistAsVector2(Vector2 v1, Vector2 v2) => v1 - v2;
    ///<summary>v1 - v2</summary>
    public static Vector2 DistAsVector2(Vector2 v1, Vector3 v2) => v1 - v2.ToVec2();
    ///<summary>v1 - v2</summary>
    public static Vector2 DistAsVector2(Vector3 v1, Vector2 v2) => v1.ToVec2() - v2;
    ///<summary>v1 - v2</summary>
    public static Vector2 DistAsVector2(Vector3 v1, Vector3 v2) => v1.ToVec2() - v2.ToVec2();

    public static bool In(this float x, float min, float max, bool inclusive = true)
    {
        if(inclusive) return x >= min && x <= max;
        else          return x >  min && x <  max;
    }

    public static Vector2 FlipX(this Vector2 vec)
    {
        return new Vector2(-vec.x, vec.y);
    }
    public static Vector2 FlipY(this Vector2 vec)
    {
        return new Vector2(vec.x, -vec.y);
    }
}
