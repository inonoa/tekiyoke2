using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//絶対に便利クラスを作ったほうがいい
public class JumpEffect : MonoBehaviour, IReusable
{
    public bool InUse{ get; private set; } = false;

    [SerializeField] Vector3 positionFromHero;
    [SerializeField] SimpleAnim[] anims;

    public void Activate(string heroDirStr){
        InUse = true;

        transform.position = HeroDefiner.CurrentHeroPos + positionFromHero;

        foreach(SimpleAnim anim in anims) anim.ResetAndStartAnim(()=>{
            gameObject.SetActive(false);
            InUse = false;
        });
    }
}
