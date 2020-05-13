using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicesLocator
{
    public IAskedInput GetInput(){
        //return new NoInputMock();
        return InputManager.Instance;
    }

    public static ServicesLocator Instance{
        get{
            if(_Instance == null) _Instance = new ServicesLocator();
            return _Instance;
        }
    }

    static ServicesLocator _Instance;
    private ServicesLocator(){ }
}

public class NoInputMock : IAskedInput{
    public void SetInputLatency(ButtonCode b, int latency){ }
    public int  GetInputLatency(ButtonCode b) => 0;
    public bool GetButton(ButtonCode b) => false;
    public bool GetButtonDown(ButtonCode b) => false;
    public bool GetButtonUp(ButtonCode b) => false;
    public bool ButtonsDownSimultaneously(ButtonCode b1, ButtonCode b2) => false;
    public bool AnyButtonDown() => false;
    public int  GetNagaoshiFrames(ButtonCode b) => 0;
}
