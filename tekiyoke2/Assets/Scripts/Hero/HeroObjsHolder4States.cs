using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>暫定的に、State内でプレハブをインスタンス化したいときにここを参照する？/多分、Stateを1個かn個しか作らないようにしてScriptableObjectにしてしまうのがいい</summary>
public class HeroObjsHolder4States : MonoBehaviour
{
    public GameObject jetstreamPrefab;
    public TrailRenderer jetTrail;

    public GameObject tsuchihokoriForRun;
}
