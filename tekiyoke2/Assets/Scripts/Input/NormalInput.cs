using System.Linq;
using UnityEngine;

public class NormalInput : MonoBehaviour, IAskedInput
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
        return settings.KeyboardSettings.ButtonsToKeys
            .Any(b2ks => GetButtonDown(b2ks.Button));
    }
}