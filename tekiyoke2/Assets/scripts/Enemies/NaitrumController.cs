using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;
using Sirenix.OdinInspector;

public class NaitrumController : MonoBehaviour, IHaveDPinEnemy, ISpawnsNearHero
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] bool toRight = false;
    Collider2Wall col;

    [Space(10)]
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Rigidbody2D RigidBody;

    [field: SerializeField, LabelText("DPCD")]
    public DPinEnemy DPCD{ get; private set; }
    

    private void Turn(object sender, EventArgs e){
        toRight = !toRight;
        foreach(SpriteRenderer sr in spriteRenderers) sr.flipX = toRight;
    }

    void Start()
    {
        col = GetComponent<Collider2Wall>();
        col.touched2Wall += Turn;
        foreach(SpriteRenderer sr in spriteRenderers) sr.flipX = toRight;
    }

    void Update() => MovePos( (toRight ? 1 : -1) * moveSpeed, 0);

    void MovePos(float v_x, float v_y)
    {
        RigidBody.MovePosition(new Vector2(RigidBody.transform.position.x + v_x * TimeManager.Current.TimeScaleExceptHero,
                                       RigidBody.transform.position.y + v_y * TimeManager.Current.TimeScaleExceptHero));
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}