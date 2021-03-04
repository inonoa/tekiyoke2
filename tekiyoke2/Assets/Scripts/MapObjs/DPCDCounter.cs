using Sirenix.OdinInspector;
using UnityEngine;

public class DPCDCounter : MonoBehaviour
{
    [Button]
    void Count()
    {
        print(GetComponentsInChildren<DPCD>().Length);
    }
}