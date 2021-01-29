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

[Serializable]
public class ButtonToKeys
{
    [SerializeField, ReadOnly, HideLabel, HorizontalGroup(Width = 65)]
    ButtonCode button;
    [SerializeField, HorizontalGroup, ListDrawerSettings(Expanded = true)]
    KeyCode[] keys;

    public ButtonCode Button => button;
    public IReadOnlyList<KeyCode> Keys => keys;

    public ButtonToKeys(ButtonCode button, KeyCode[] keys)
    {
        this.button = button;
        this.keys = keys;
    }
}
