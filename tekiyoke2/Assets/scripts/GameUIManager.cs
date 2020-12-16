using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] JetCloudManager _JetCloud;
    public JetCloudManager JetCloud{ get => _JetCloud; }
    [SerializeField] Chishibuki _Chishibuki;
    public Chishibuki Chishibuki{ get => _Chishibuki; }


    #region Locator(暫定)
    public static GameUIManager CurrentInstance{ get; private set; }
    void Awake() => CurrentInstance = this;

    #endregion
}
