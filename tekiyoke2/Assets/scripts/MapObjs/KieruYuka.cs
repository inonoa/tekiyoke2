using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KieruYuka : MonoBehaviour
{
    KieruYukaCntr cntr;
    public event EventHandler heroOn;

    void Start()
    {
        cntr = transform.parent.gameObject.GetComponent<KieruYukaCntr>();
        heroOn += cntr.AddYuka;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Player"){
            heroOn?.Invoke(this,EventArgs.Empty);
        }
    }
}
