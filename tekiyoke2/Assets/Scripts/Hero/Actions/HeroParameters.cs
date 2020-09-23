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
    [SerializeField] MoveInAirParams _MoveinAirParams;
    [SerializeField] float _JumpForce = 30f;

    public float GroundSpeedMax => _GroundSpeedMax;
    public float ForceOnGround => _ForceOnGround;
    public float Friction => _Friction;
    public MoveInAirParams MoveInAirParams => _MoveinAirParams;
    public float JumpForce => _JumpForce;
}

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
