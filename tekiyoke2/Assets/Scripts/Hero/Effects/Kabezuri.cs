using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Kabezuri : MonoBehaviour, IReusable
{
    [SerializeField] Vector3 positionFromHero;
    SimpleAnim anim;
    SpriteRenderer spriteRenderer; 

    Tween[] tweensToKill = new Tween[3];
    
    void Awake()
    {
        anim = GetComponent<SimpleAnim>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Activate(string wallDirStr){
        InUse = true;

        foreach(Tween tw in tweensToKill) tw?.Kill();

        bool dir_is_R = wallDirStr=="r";

        transform.position = HeroDefiner.CurrentHeroPos + new Vector3(
            dir_is_R ? positionFromHero.x : -positionFromHero.x,
            positionFromHero.y,
            positionFromHero.z
        );
        transform.localScale = new Vector3(dir_is_R ? 1 : -1, 1, 1);
        spriteRenderer.color = new Color(1,1,1,1);

        anim.ResetAndStartAnim(() => {
            tweensToKill[0] = transform.DOMoveY(100,0.5f).SetRelative().AsHeros();
            tweensToKill[1] = spriteRenderer.DOFade(0, 0.5f).AsHeros();
            tweensToKill[2] = DOVirtual.DelayedCall(0.5f, () => {
                gameObject.SetActive(false);
                InUse = false;
            }, ignoreTimeScale: false).AsHeros();
        });
    }

    public bool InUse{ get; private set; }
}
