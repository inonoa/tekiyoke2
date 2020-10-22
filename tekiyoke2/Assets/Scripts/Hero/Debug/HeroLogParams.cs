using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroLogParams", menuName = "Scriptable Object/Hero Log Params", order = 100)]
public class HeroLogParams : ScriptableObject
{
    [SerializeField] bool _State = false;
    [SerializeField] bool _Velocity = false;
    [SerializeField] bool _KeyDirection = false;
    [SerializeField] bool _WantsToGoRight = false;
    [SerializeField] bool _IsOnGround = false;


    public bool State => _State;
    public bool Velocity => _Velocity;
    public bool KeyDirection => _KeyDirection;
    public bool WantsToGoRight => _WantsToGoRight;
    public bool IsOnGround => _IsOnGround;
}
