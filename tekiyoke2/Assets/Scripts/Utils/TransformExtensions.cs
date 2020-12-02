
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
}