using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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