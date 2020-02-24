using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public class WindPath : MonoBehaviour
{
    [SerializeField] float delaySecAfterScSho = 0;
    [SerializeField] float moveSec = 1;
    [SerializeField] [TextArea(5,20)] string pathStr = "";
    Vector3[] pathVecs;

    void Start()
    {

        DOVirtual.DelayedCall(delaySecAfterScSho, () => {
            pathVecs = Str2LocalVecs(pathStr);
            transform.DOPath(pathVecs, moveSec, PathType.Linear);
        });
    }

    Vector3[] Str2LocalVecs(string str){

        string[] pointStrs = str.Split(new string[]{"\n"}, StringSplitOptions.RemoveEmptyEntries);
        return pointStrs.Select(Str2LocalVec).ToArray();

        Vector3 Str2LocalVec(string vecStr){
            string[] xyStrs = vecStr.Split(',');
            return new Vector3(int.Parse(xyStrs[0]) + transform.parent.position.x,
                               int.Parse(xyStrs[1]) + transform.parent.position.y, -400);
        }
    }
}
