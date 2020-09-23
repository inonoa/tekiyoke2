using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>落下スピードに上限を付けようとしていつの間にかクラス化してしまった、過剰かもしれない、あとバネの余韻とかその辺は入らない</summary>
public class HeroVelocity
{
    static readonly float minY = -50;
    float _X, _Y;
    public float X{
        get => _X;
        set{
            _X = value;
        }
    }
    public float Y{
        get => _Y;
        set{
            _Y = (value <= minY) ? minY : value;
        }
    }
    public HeroVelocity(float x, float y){
        (X, Y) = (x, y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public float Magnitude()
    {
        return Mathf.Sqrt(X*X + Y*Y);
    }

    public static HeroVelocity operator + (HeroVelocity v1, HeroVelocity v2){
        return new HeroVelocity(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static HeroVelocity operator - (HeroVelocity v1, HeroVelocity v2){
        return new HeroVelocity(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static HeroVelocity operator * (HeroVelocity v, float a)
    {
        return new HeroVelocity(v.X * a, v.Y * a);
    }
    public static HeroVelocity operator * (float a, HeroVelocity v)
    {
        return v * a;
    }
}
