using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class KazagurumaObserver : MonoBehaviour
{
    [SerializeField] Kazaguruma[] kazagurumas;

    public event EventHandler AllRotated;
    bool allRotating = false;
    public bool AllRotating{
        get => allRotating;
    }

    // Start is called before the first frame update
    void Start()
    {
        AllRotated += (s, e) => print("回った！！");
    }

    // Update is called once per frame
    void Update()
    {
        bool alro = kazagurumas.All(kg => kg.IsRotating);
        if(alro && !AllRotating) AllRotated?.Invoke(this, EventArgs.Empty);
        allRotating = alro;
    }
}
