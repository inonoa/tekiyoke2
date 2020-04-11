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

    public void Activate(string r_l_kr_kl){
        InUse = true;

        transform.position = HeroDefiner.CurrentHeroPos + positionFromHero;
        transform.rotation = Quaternion.identity;
        if(r_l_kr_kl[0]=='k') transform.Rotate(0, 0, r_l_kr_kl[1]=='r' ? -40 : 40);

        foreach(SimpleAnim anim in anims) anim.ResetAndStartAnim(()=>{
            gameObject.SetActive(false);
            InUse = false;
        });
    }
}
