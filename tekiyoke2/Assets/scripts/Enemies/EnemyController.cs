using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] DPinEnemy dpcd;
    public DPinEnemy DPCD => dpcd;

    [SerializeField] protected Rigidbody2D rBody;

    protected TimeManager TimeManager{ get; private set; }

    protected void MovePos(float v_x, float v_y)
    {
        rBody.MovePosition(new Vector2(rBody.transform.position.x + v_x * TimeManager.CurrentInstance.TimeScaleExceptHero,
                                       rBody.transform.position.y + v_y * TimeManager.CurrentInstance.TimeScaleExceptHero));
    }

    ///<summary>指定したv_xだけ横に移動する。y軸方向には重力のままに移動する。</summary>
    protected void MoveX_ConsideringGravity(float v_x, float fallSpeedMax = 300)
    {
        rBody.velocity = new Vector2(v_x * TimeManager.CurrentInstance.TimeScaleExceptHero, Math.Max(rBody.velocity.y, -fallSpeedMax));
    }

    protected void Init()
    {
        TimeManager = TimeManager.CurrentInstance;
    }

    protected void Update_() => rBody.velocity = new Vector2(0, rBody.velocity.y);

    public virtual void OnSpawned(){ }
}
