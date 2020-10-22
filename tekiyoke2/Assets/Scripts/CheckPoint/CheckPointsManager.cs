using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;

public class CheckPointsManager : MonoBehaviour
{
    [SerializeField] CheckPoint[] checkPoints;
    [SerializeField] [ReadOnly] int frontLine = -1;

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
                if(frontLine < index)
                {
                    frontLine = index;
                    // "チェックポイント通過"
                    print($"チェックポイント: {cp.Name} を通過");
                    MemoryOverDeath.Instance.PassCheckPoint(index);
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
            })
            .AddTo(this);
        }
    }

    void Awake()
    {
        Instance = this;
    }
    public static CheckPointsManager Instance{ get; private set; }
}
