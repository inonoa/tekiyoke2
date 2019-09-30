using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//主人公をいちいちInspector上なりなんなりで指定するのは面倒なのでここで一括管理すればええんでは
public class HeroDefiner : MonoBehaviour
{
    static public HeroMover currentHero;

    static public Vector3 CurrentHeroPos{
        get{ return currentHero.transform.position; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
