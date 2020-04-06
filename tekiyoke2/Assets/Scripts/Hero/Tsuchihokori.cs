using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsuchihokori : MonoBehaviour, IReusable
{

    [SerializeField] Vector3 positionFromHero;
    public void Activate(){
        InUse = true;
        transform.position = HeroDefiner.CurrentHeroPos + positionFromHero;
    }

    public bool InUse{ get; private set; }
}
