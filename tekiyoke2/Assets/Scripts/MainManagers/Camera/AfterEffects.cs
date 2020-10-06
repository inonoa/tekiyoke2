using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class AfterEffects : MonoBehaviour
{

    [SerializeField] PostEffectWrapper[] effects;
    public PostEffectWrapper Find(string key) => effects.Find(key);

    void Awake()
    {
        Camera camera = GetComponent<Camera>();

        effects.ForEach(ef =>
        {
            ef.Init(camera);
            if(ef.IsActive) ef.ApplyCommandBuf();
            ef.BecomeDirty.Subscribe(_ => RearrangeBuffers(ef));
        });
    }

    void RearrangeBuffers(PostEffectWrapper dirty)
    {
        if(dirty.IsActive)
        {
            effects.Reverse()
                .TakeWhile(ef => ef != dirty)
                .Where    (ef => ef.IsActive)
                .ForEach  (ef => ef.RemoveCommandBuf());

            dirty.ApplyCommandBuf();

            effects
                .SkipWhile(ef => ef != dirty)
                .Skip(1)
                .Where    (ef => ef.IsActive)
                .ForEach  (ef => ef.ApplyCommandBuf());
        }
        else
        {
            dirty.RemoveCommandBuf();
        }
    }

    void Update() => effects.ForEach(ef => ef.Update_());

}
