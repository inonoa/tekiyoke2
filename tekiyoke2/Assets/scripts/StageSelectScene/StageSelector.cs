using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour
{   
    #region GameObjects
    public GameObject waku;
    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;

    public GameObject curtain;

    #endregion

    enum State{
        Entering, Active, Selected
    }
    State state = State.Entering;
    int selected = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
