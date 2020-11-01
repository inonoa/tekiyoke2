using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;

public class JumpCounter : MonoBehaviour
{
    [SerializeField] HeroMover hero;

    [field: SerializeField, LabelText("Can Jump In Air"), ReadOnly]
    public bool CanJumpInAir{ get; private set; } = true;
    
    void Start()
    {
        hero.OnLand
            .Subscribe(_ => CanJumpInAir = true);
        hero.OnJumped
            .Where(jump => !jump.isFromGround)
            .Subscribe(jump => CanJumpInAir = false);
    }
}
