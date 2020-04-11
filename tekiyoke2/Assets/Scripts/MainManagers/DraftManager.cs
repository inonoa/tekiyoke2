using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftManager : MonoBehaviour
{
    public static DraftManager CurrentInstance{ get; private set; }

    [SerializeField] Transform _GameMasterTF;
    public Transform GameMasterTF => _GameMasterTF;
    [SerializeField] Transform _PauseMasterTF;
    public Transform PauseMasterTF => _PauseMasterTF;
    
    void Awake()
    {
        CurrentInstance = this;
    }
}
