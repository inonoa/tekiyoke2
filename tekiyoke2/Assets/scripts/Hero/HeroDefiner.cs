using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>主人公の参照を良い感じに持てるようになるのは良いが若干危険なのかなあ、あとHeroMoverに書けばよかったかもなあ</summary>
public class HeroDefiner
{
    static public HeroMover currentHero;
    static public Vector3 CurrentPos{ get => currentHero.transform.position; }
    static public RingBuffer<Vector3> PastPoss{ get => currentHero.pastPoss; }
    static public Vector2 ExpectedPos{ get => new Vector2(currentHero.expectedPosition.x, currentHero.expectedPosition.y); }
}
