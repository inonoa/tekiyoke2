using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "HeroParameters", menuName = "Scriptable Object/Hero Parameters", order = 100)]
public class HeroParameters : ScriptableObject
{
               [SerializeField] RunParams _RunParams;
    [Space(5)] [SerializeField] MoveInAirParams _MoveInAirParams;
    [Space(5)] [SerializeField] MoveInAirParams _MoveInAirParamsAfterKick;
    [Space(5)] [SerializeField] float _JumpForce = 30f;
    [Space(5)] [SerializeField] KickParams _KickParams;
    [Space(5)] [SerializeField] JetParams _JetParams;
    [Space(5)] [SerializeField] BendParams _BendParams;
    [Space(5)] [SerializeField] float _MutekiSeconds = 2f;

    public RunParams RunParams             => _RunParams;
    public MoveInAirParams MoveInAirParams => _MoveInAirParams;
    public MoveInAirParams MoveInAirParamsAfterKick => _MoveInAirParamsAfterKick;
    public float JumpForce                 => _JumpForce;
    public KickParams KickParams           => _KickParams;
    public JetParams JetParams             => _JetParams;
    public BendParams BendParams           => _BendParams;
    public float MutekiSeconds             => _MutekiSeconds;
}

public enum KickKey{ DirOfWall, DirAgainstWall, Any }

[Serializable]
public class RunParams
{
    [SerializeField] float _GroundSpeedMax = 15f;
    [SerializeField] float _ForceOnGround = 200f;
    [SerializeField] float _Friction = 150f;
    [SerializeField] float _CoyoteTime = 0.1f;
    [SerializeField] float _TsuchihokoriInterval = 0.12f;

    
    public float GroundSpeedMax            => _GroundSpeedMax;
    public float ForceOnGround             => _ForceOnGround;
    public float Friction                  => _Friction;
    public float CoyoteTime                => _CoyoteTime;
    public float TsuchihokoriInterval      => _TsuchihokoriInterval;
}

[Serializable]
public class MoveInAirParams
{
    [SerializeField] float _HorizontalForce      = 200f;
    [SerializeField] float _HorizontalResistance = 7f;
    [SerializeField] float _HorizontalSpeedMax   = 15f;
    [SerializeField] float _Gravity              = 80f;
    [SerializeField] float _FallSpeedMax         = 50f;
    [SerializeField] float _KabezuriInterval     = 0.1f;

    public float HorizontalForce => _HorizontalForce;
    public float HorizontalResistance => _HorizontalResistance;
    public float HorizontalSpeedMax => _HorizontalSpeedMax;
    public float Gravity => _Gravity;
    public float FallSpeedMax => _FallSpeedMax;
    public float KabezuriInterval => _KabezuriInterval;
}

[Serializable]
public class KickParams
{
    [SerializeField] Vector2 _WallKickForce        = new Vector2(15f, 30f);
    [SerializeField] KickKey _WallKickKey          = KickKey.DirAgainstWall;
    [SerializeField] float _FromKickToInputEnabled = 0.3f;

    public Vector2 KickForce            => _WallKickForce;
    public KickKey KickKey              => _WallKickKey;
    public float FromKickToInputEnabled => _FromKickToInputEnabled;
}

[Serializable]
public class JetParams
{
    [SerializeField] float _MinDistance          = 200;
    [SerializeField] float _MaxDistance          = 700;
    [SerializeField] float _JetSecondsMin        = 0.2f;
    [SerializeField] float _JetSecondsMax        = 0.5f;
    [SerializeField] float _ChargeSecondsFromMin = 0.3f;
    [SerializeField] float _ChargeSecondsToMax   = 1.6f;
    [SerializeField] float _CoolTime             = 0.5f;
    [SerializeField] float _TimeScaleBeforeJet   = 0.1f;
    [SerializeField] float _TweenLinearRate      = 0.05f;

    public float MinDistance          => _MinDistance;
    public float MaxDistance          => _MaxDistance;
    public float JetSecondsMax        => _JetSecondsMax;
    public float JetSecondsMin        => _JetSecondsMin;
    public float ChargeSecondsFromMin => _ChargeSecondsFromMin;
    public float ChargeSecondsToMax   => _ChargeSecondsToMax;
    public float CoolTime             => _CoolTime;
    public float TimeScaleBeforeJet   => _TimeScaleBeforeJet;
    public float TweenLinearRate      => _TweenLinearRate;
}

[Serializable]
public class BendParams
{
    [SerializeField] Vector2 _BendBackForce = new Vector2(-15, 15);
    [SerializeField] float   _Gravity       = 40f;
    [SerializeField] float _BendBackSeconds = 0.6f;

    public Vector2 BendBackForce => _BendBackForce;
    public float   Gravity       => _Gravity;
    public float BendBackSeconds => _BendBackSeconds;
}
