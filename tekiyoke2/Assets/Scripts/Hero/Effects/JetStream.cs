using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class JetStream : MonoBehaviour
{
    PolygonCollider2D windTrigger;
    new TrailRenderer renderer;
    HeroMover hero;
    Vector2 posWhenJet;
    
    void Awake()
    {
        windTrigger = GetComponent<PolygonCollider2D>();
        renderer = GetComponent<TrailRenderer>();
    }

    public void Init(HeroMover hero)
    {
        this.hero = hero;
        posWhenJet = hero.transform.position;
        transform.position = hero.transform.position;

        hero.JetManager.JetEnded
            .Subscribe(_ => Destroy(gameObject))
            .AddTo(this);
    }
    
    void Update()
    {
        transform.position = hero.transform.position;
        
        List<Vector2> points = new List<Vector2>();

        points.Add(new Vector2(0,  40));
        points.Add(new Vector2(0, -40));

        Vector2 posWhenJetRelative = posWhenJet - transform.position.ToVec2();
        points.Add(posWhenJetRelative + new Vector2(0, -40));
        points.Add(posWhenJetRelative + new Vector2(0,  40));

        windTrigger.points = points.ToArray();
    }
}
