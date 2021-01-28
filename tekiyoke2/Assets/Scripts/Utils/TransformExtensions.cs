
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;

public static class TransformExtensions
{
    public static void SetX(this Transform transform, float x)
    {
        Vector3 tmp = transform.position;
        tmp.x = x;
        transform.position = tmp;
    }
    
    public static void SetY(this Transform transform, float y)
    {
        Vector3 tmp = transform.position;
        tmp.y = y;
        transform.position = tmp;
    }
    
    public static void SetZ(this Transform transform, float z)
    {
        Vector3 tmp = transform.position;
        tmp.z = z;
        transform.position = tmp;
    }
    public static void SetLocalX(this Transform transform, float x)
    {
        Vector3 tmp = transform.localPosition;
        tmp.x = x;
        transform.localPosition = tmp;
    }
    
    public static void SetLocalY(this Transform transform, float y)
    {
        Vector3 tmp = transform.localPosition;
        tmp.y = y;
        transform.localPosition = tmp;
    }
    
    public static void SetLocalZ(this Transform transform, float z)
    {
        Vector3 tmp = transform.localPosition;
        tmp.z = z;
        transform.localPosition = tmp;
    }

    public static void AddLocalX(this Transform transform, float dx)
    {
        transform.localPosition += new Vector3(dx, 0, 0);
    }
    
    public static void AddLocalY(this Transform transform, float dy)
    {
        transform.localPosition += new Vector3(0, dy, 0);
    }
    
    public static void AddLocalZ(this Transform transform, float dz)
    {
        transform.localPosition += new Vector3(0, 0, dz);
    }

    public static Tween DOMyRotate(this Transform transform, float target, float duration, bool clockwise)
    {
        float angle = transform.rotation.eulerAngles.z;
        float actualTarget = ActualTargetAngle(angle, target, clockwise);
        return DOTween.To
        (
            ()  => angle,
            val =>
            {
                angle = val;
                transform.rotation = Quaternion.Euler(0, 0, val);
            },
            actualTarget,
            duration
        );
    }
    
    //TransformExtensionsとは…………？
    public static Tween DOMyRotate(this Rigidbody2D rigidBody, float target, float duration, bool clockwise)
    {
        float angle = (rigidBody.rotation + 360) % 360;
        float actualTarget = ActualTargetAngle(angle, target, clockwise);
        return DOTween.To
        (
            ()  => angle,
            val =>
            {
                angle = val;
                rigidBody.SetRotation(val);
            },
            actualTarget,
            duration
        );
    }

    static float ActualTargetAngle(float from, float target, bool clockwise)
    {
        if (clockwise)
        {
            return target > from ? target - 360 : target;
        }
        else
        {
            return target < from ? target + 360 : target;
        }
    }
}