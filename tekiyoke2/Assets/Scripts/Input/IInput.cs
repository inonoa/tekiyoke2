using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    bool GetButton(ButtonCode b);
    bool GetButtonDown(ButtonCode b);
    bool GetButtonUp(ButtonCode b);
    bool AnyButtonDown();
}

public enum ButtonCode
{
    Right, Left, Up, Down,
    Jump, Jet,
    Zone, Pause, Enter, Cancel,
    Tweet, Ranking
}
