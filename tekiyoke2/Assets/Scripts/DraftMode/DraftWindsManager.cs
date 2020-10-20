using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Draft
{
    public class DraftWindsManager : MonoBehaviour
    {
        WindsMover mover;
        new WindsRenderer renderer;

        void Awake()
        {
            mover    = GetComponent<WindsMover>();
            renderer = GetComponent<WindsRenderer>();
        }

        public void Init(Func<HeroInfo> heroInfoGetter)
        {
            mover.Init(heroInfoGetter);
        }

        public void SetActive(bool active)
        {
            renderer.active = active;
            mover.updates   = active;
        }
        public bool IsActive()
        {
            Debug.Assert(renderer.active == mover.updates);
            return renderer.active;
        }
    }
}
