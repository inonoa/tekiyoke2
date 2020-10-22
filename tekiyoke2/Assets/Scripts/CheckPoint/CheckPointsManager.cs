using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;

public class CheckPointsManager : MonoBehaviour
{
    [SerializeField] CheckPoint[] checkPoints;

    [SerializeField] [ReadOnly] int frontLine = -1;
    
    void Start()
    {
        for(int i = 0; i < checkPoints.Length; i++)
        {
            int index = i;
            checkPoints[index].Passed.Subscribe(cp =>
            {
                if(frontLine < index)
                {
                    frontLine = index;
                    // "チェックポイント通過"
                    print($"チェックポイント: {cp.Name}");
                }
                else if(frontLine > index)
                {
                    // "既に奥のチェックポイントを通過しています"
                    // これ必要かなあ
                    print($"チェックポイント: {cp.Name} より奥のチェックポイントを通過済み");
                }
                else
                {
                    Debug.LogError($"チェックポイント: {cp.Name} に二回目の通過！！");
                }
            });
        }
    }

    
    void Update()
    {
        
    }
}
