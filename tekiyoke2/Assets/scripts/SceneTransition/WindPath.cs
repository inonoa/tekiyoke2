using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;
using Sirenix.OdinInspector;

public class WindPath : MonoBehaviour
{
    [SerializeField] float delaySecAfterScSho = 0;
    [SerializeField] float moveSec = 1;
    [SerializeField] float pathZ;
    
    [SerializeField] [TextArea(5,20)] string pathStr = "";
    [SerializeField, ReadOnly] Vector2[] pathVecs;
    [Button]
    void ApplyPathStr() => pathVecs = Str2Vecs(pathStr);

    void Start()
    {
        DOVirtual.DelayedCall(delaySecAfterScSho, () =>
        {
            var parentPos = transform.parent.position;
            var path = pathVecs
                       .Select(v2 => new Vector3
                       (
                           v2.x + parentPos.x,
                           v2.y + parentPos.y,
                           pathZ
                       ))
                       .ToArray();
            
            transform.DOPath(path, moveSec, PathType.CatmullRom);
        });
    }

    static Vector2[] Str2Vecs(string str)
    {
        return str.Split(new []{ "\n" }, StringSplitOptions.RemoveEmptyEntries)
                  .Select(line =>
                  {
                      string[] xyStr = line.Split(',');
                      return new Vector2(float.Parse(xyStr[0]), float.Parse(xyStr[1]));
                  })
                  .ToArray();
    }

    static string Vecs2Str(Vector2[] vecs)
    {
        return string.Join("\n", vecs.Select(vec => $"{vec.x},{vec.y}"));
    }

    [Button]
    void DebugRescale(float rate)
    {
        pathVecs = Str2Vecs(pathStr).Select(vec => vec * rate).ToArray();
        pathStr = Vecs2Str(pathVecs);
    }
}
