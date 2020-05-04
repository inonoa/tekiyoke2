using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCameraController : MonoBehaviour
{
    void OnEnable() => gameObject.AddComponent<AudioListener>();
    void OnDisable() => Destroy(GetComponent<AudioListener>());
}
