using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "HeroParameters", menuName = "Scriptable Object/Hero Parameters", order = 100)]
public class HeroParameters : ScriptableObject
{
    [SerializeField] float _GroundSpeedMax = 15f;
    [SerializeField] float _ForceOnGround = 200f;
    [SerializeField] float _Friction = 150f;
    [SerializeField] float _CoyoteTime = 0.1f;
    [Space(5)] [SerializeField] MoveInAirParams _MoveInAirParams;
    [Space(15)] [SerializeField] float _JumpForce = 30f;
    [Space(5)] [SerializeField] KickParams _KickParams;


    public float GroundSpeedMax => _GroundSpeedMax;
    public float ForceOnGround => _ForceOnGround;
    public float Friction => _Friction;
    public float CoyoteTime => _CoyoteTime;
    public MoveInAirParams MoveInAirParams => _MoveInAirParams;
    public float JumpForce => _JumpForce;
    public KickParams KickParams => _KickParams;
}

public enum KickKey{ DirOfWall, DirAgainstWall }

[Serializable]
public class MoveInAirParams
{
    [SerializeField] float _HorizontalForce = 200f;
    [SerializeField] float _HorizontalResistance = 7f;
    [SerializeField] float _HorizontalSpeedMax = 15f;
    [SerializeField] float _Gravity = 80f;
    [SerializeField] float _FallSpeedMax = 50f;

    public float HorizontalForce => _HorizontalForce;
    public float HorizontalResistance => _HorizontalResistance;
    public float HorizontalSpeedMax => _HorizontalSpeedMax;
    public float Gravity => _Gravity;
    public float FallSpeedMax => _FallSpeedMax;
    
}

[Serializable]
public class KickParams
{
    [SerializeField] Vector2 _WallKickForce = new Vector2(15f, 30f);
    [SerializeField] KickKey _WallKickKey = KickKey.DirAgainstWall;
    [SerializeField] float _FromKickToInputEnabled = 0.3f;

    public Vector2 KickForce => _WallKickForce;
    public KickKey KickKey => _WallKickKey;
    public float FromKickToInputEnabled => _FromKickToInputEnabled;
}
