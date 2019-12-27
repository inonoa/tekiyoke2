using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DontWannaFall : MonoBehaviour
{
    public event EventHandler about2fall;

    [SerializeField]
    bool isR = true;

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Terrain") about2fall?.Invoke(isR, EventArgs.Empty);
    }
}
