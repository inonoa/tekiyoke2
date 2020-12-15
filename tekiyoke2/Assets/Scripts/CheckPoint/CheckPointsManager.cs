using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using System;
using System.Linq;

public class CheckPointsManager : MonoBehaviour
{
    CheckPoint[] checkPoints;

    [SerializeField, ReadOnly] int frontLine = -1;

    Subject<CheckPoint> _Passed = new Subject<CheckPoint>();
    public IObservable<CheckPoint> PassedNewCheckPoint => _Passed;
    
    void Awake()
    {
        Instance = this;
        checkPoints = GetComponentsInChildren<CheckPoint>();
        foreach(int i in Enumerable.Range(0, checkPoints.Length))
        {
            checkPoints[i].Init(i);
        }
    }

    public Vector2 GetPosition(int index)
    {
        return checkPoints[index].transform.position;
    }
    
    public void Init(int frontLineIndex)
    {
        frontLine = frontLineIndex;

        for(int i = frontLine + 1; i < checkPoints.Length; i++)
        {
            int index = i;
            checkPoints[index].Passed.Subscribe(cp =>
            {
                Debug.Assert(frontLine < index, "不正なチェックポイントを通っています");
                
                frontLine = index;
                MemoryOverDeath.Instance.PassCheckPoint(index);
                
                _Passed.OnNext(cp);
            })
            .AddTo(this);
        }
    }
    
    
    public static CheckPointsManager Instance{ get; private set; }
}
