using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAskedInput
{
    void SetInputLatency(ButtonCode b, int latency);
    int  GetInputLatency(ButtonCode b);
    bool GetButton(ButtonCode b);
    bool GetButtonDown(ButtonCode b);
    bool GetButtonUp(ButtonCode b);
    bool ButtonsDownSimultaneously(ButtonCode b1, ButtonCode b2);
    bool AnyButtonDown();
    int  GetNagaoshiFrames(ButtonCode b);

}

public enum ButtonCode{
    Right, Left, Up, Down,
    Jump, JetL, JetR, JetLR,
    Zone, Save, Pause, Enter, Cancel,
    Tweet
}
