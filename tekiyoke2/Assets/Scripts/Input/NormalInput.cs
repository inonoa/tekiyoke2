using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalInput", menuName = "Scriptable Object/NormalInput", order = 0)]
public class NormalInput : ScriptableObject, IAskedInput
{
    [SerializeField] InputSettings settings;
    
    public bool GetButton(ButtonCode b)
    {
        return settings.KeyboardSettings[b].Any(Input.GetKey);
    }

    public bool GetButtonDown(ButtonCode b)
    {
        return settings.KeyboardSettings[b].Any(Input.GetKeyDown);
    }

    public bool GetButtonUp(ButtonCode b)
    {
        return settings.KeyboardSettings[b].Any(Input.GetKeyUp);
    }

    public bool AnyButtonDown()
    {
        return settings.AllKeys().Any(Input.GetKeyDown);
    }
}