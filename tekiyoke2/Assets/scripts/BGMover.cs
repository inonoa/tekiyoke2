using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMover : MonoBehaviour
{

    public HeroMover hero;
    public float bgrate = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        hero = gameObject.GetComponentInParent<HeroMover>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(hero.transform.position.x/(1+bgrate),hero.transform.position.y/(1+bgrate)+50);
    }
}
