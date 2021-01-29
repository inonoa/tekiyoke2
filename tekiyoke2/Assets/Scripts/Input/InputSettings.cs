using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Scriptable Object/InputSettings", order = 0)]
public class InputSettings : ScriptableObject
{
    [SerializeField] KeyboardSettings keyboardSettings;
    public KeyboardSettings KeyboardSettings => keyboardSettings;
}