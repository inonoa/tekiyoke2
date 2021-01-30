using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class KeyboardSettings
{
    [SerializeField, ListDrawerSettings(IsReadOnly = true)]
    ButtonToKeys[] buttonsToKeys;

    public IReadOnlyList<KeyCode> this[ButtonCode button]
    {
        get => buttonsToKeys[(int)button].Keys;
    }

    public KeyboardSettings()
    {
        buttonsToKeys = Enum.GetValues(typeof(ButtonCode))
            .Cast<ButtonCode>()
            .Select(button => new ButtonToKeys(button, new KeyCode[0]))
            .ToArray();
    }
}
