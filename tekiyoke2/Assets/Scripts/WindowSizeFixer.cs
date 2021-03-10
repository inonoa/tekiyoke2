using System;
using UnityEngine;

public class WindowSizeFixer : MonoBehaviour
{
    [SerializeField] int width  = 1024;
    [SerializeField] int height = 768;
    void Awake()
    {
        Screen.SetResolution(width, height, FullScreenMode.Windowed);
    }
}
