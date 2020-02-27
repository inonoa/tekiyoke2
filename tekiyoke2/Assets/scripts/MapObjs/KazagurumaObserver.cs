using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class KazagurumaObserver : MonoBehaviour
{
    [SerializeField] Kazaguruma[] kazagurumas;

    public event EventHandler AllRotated;
    public event EventHandler NotAllRotated;
    public bool AllRotating{ get; private set; }


    void Update()
    {
        bool alro = kazagurumas.All(kg => kg.IsRotating);
        if( alro && !AllRotating) AllRotated?.Invoke(this, EventArgs.Empty);
        if(!alro &&  AllRotating) NotAllRotated?.Invoke(this, EventArgs.Empty);
        AllRotating = alro;
    }
}
