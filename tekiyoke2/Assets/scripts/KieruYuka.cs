using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KieruYuka : MonoBehaviour
{
    KieruYukaCntr cntr;
    public event EventHandler heroOn;

    // Start is called before the first frame update
    void Start()
    {
        cntr = transform.parent.gameObject.GetComponent<KieruYukaCntr>();
        heroOn += cntr.AddYuka;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="Player"){
            heroOn?.Invoke(this,EventArgs.Empty);
        }
    }
}
