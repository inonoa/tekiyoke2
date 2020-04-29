using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    ///<summary></summary>
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Player"){
            Destroy(this.gameObject);
            HeroDefiner.currentHero.hpcntr.ChangeHP(HeroDefiner.currentHero.hpcntr.HP + 1);
        }
    }
}
