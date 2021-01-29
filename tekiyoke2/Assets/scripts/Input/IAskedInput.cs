using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GetButton系以外は切り離せそう
public interface IAskedInput
{
    bool GetButton(ButtonCode b);
    bool GetButtonDown(ButtonCode b);
    bool GetButtonUp(ButtonCode b);
    bool AnyButtonDown();
}

public enum ButtonCode
{
    Right, Left, Up, Down,
    Jump, JetL, JetR, JetLR,
    Zone, Save, Pause, Enter, Cancel,
    Tweet
}
