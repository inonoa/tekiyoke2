using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Scriptable Object/InputSettings", order = 0)]
public class InputSettings : ScriptableObject
{
    [SerializeField] KeyboardSettings keyboardSettings;
    public KeyboardSettings KeyboardSettings => keyboardSettings;
    
    [SerializeField] KeyCode[] exceptionalKeyCodes = new[]
    {
        KeyCode.None,
        KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2,
        KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5,
        KeyCode.Mouse6
    };
    public IEnumerable<KeyCode> AllKeys()
    {
        return Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Except(exceptionalKeyCodes);
    }
}