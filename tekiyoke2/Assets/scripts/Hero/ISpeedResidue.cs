using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpeedResidue
{
    bool IsActive{ get; }
    Vector2 UpdateVel(Vector2 currentVeclocity, float deltatime, HeroMover hero);
}
